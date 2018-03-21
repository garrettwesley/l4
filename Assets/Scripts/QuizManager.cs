using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class QuizManager : MonoBehaviour
{
    static class Constants
    {
        public const int MaxNumOptions = 3;
    }

    private int currentQuestion;
    private GameObject instantiatedQuiz;
    private Quiz quiz;
    private Vector3 originalCameraPosition;
    private int orignalCullingMask;

    public int QuizObjectsLayer = 9;
    public Font Font;
    public GameObject QuizPrefab;
    public Canvas Canvas;
    public AwsMobileAnalytics AwsMobileAnalytics;
    public FirstPersonController FirstPersonController;

    // ------------------------------------------------------------------------------------- //

    public void LoadQuiz(TextAsset quizToLoad)
    {
        this.quiz = JsonUtility.FromJson<Quiz>(quizToLoad.text);
        this.instantiatedQuiz = GameObject.Instantiate(this.QuizPrefab);
        this.instantiatedQuiz.transform.SetParent(this.Canvas.transform);

        if (!string.IsNullOrEmpty(this.quiz.ImagePath))
        {
            Sprite s = Resources.Load<Sprite>(this.quiz.ImagePath);
            Image img = this.instantiatedQuiz.transform.Find("Image").GetComponent<Image>();
            img.gameObject.SetActive(true);
            img.sprite = s;
            img.rectTransform.sizeDelta = new Vector2(s.texture.width, s.texture.height);
            img.rectTransform.anchoredPosition = new Vector2(0, -s.texture.height / 2);
        }

        if (this.FirstPersonController != null &&
            this.quiz.CameraLocationX != 0 &&
            this.quiz.CameraLocationY != 0 &&
            this.quiz.CameraLocationZ != 0)
        {
            this.originalCameraPosition = this.FirstPersonController.transform.position;
            Debug.Log(string.Format("Setting fps position to {0}", new Vector3(this.quiz.CameraLocationX, this.quiz.CameraLocationY, this.quiz.CameraLocationZ)));
            this.FirstPersonController.transform.position = new Vector3(this.quiz.CameraLocationX, this.quiz.CameraLocationY, this.quiz.CameraLocationZ);
            Camera c = this.FirstPersonController.GetComponentInChildren<Camera>();
            c.gameObject.transform.LookAt(
                new Vector3(
                    this.quiz.CameraLookAtX,
                    this.quiz.CameraLookAtY,
                    this.quiz.CameraLookAtZ));
            this.FirstPersonController.GetComponent<FirstPersonController>().enabled = false;
            this.orignalCullingMask = c.cullingMask;
            c.cullingMask = 1 << this.QuizObjectsLayer;
        }

        this.currentQuestion = -1;
        LoadNextQuestion();
    }

    // ------------------------------------------------------------------------------------- //

    public void LoadNextQuestion()
    {
        Question q = this.quiz.Questions[++this.currentQuestion];
        
        Text t = this.instantiatedQuiz.transform.Find("QuestionText").GetComponent<Text>();
        t.rectTransform.sizeDelta = new Vector2(Screen.width, t.rectTransform.sizeDelta.y);
        t.text = q.Text;

        Button button = this.instantiatedQuiz.transform.Find("Option1").GetComponent<Button>();

        float w = Screen.width;
        float n = q.Options.Length;
        float bw = button.GetComponent<RectTransform>().sizeDelta.x;
        float s = (w - (n * bw)) / (n + 1);

        for (int i = 1; i <= q.Options.Length; i++)
        {
            Button b = this.instantiatedQuiz.transform.Find("Option" + i).GetComponent<Button>();
            b.gameObject.SetActive(true);
            RectTransform rt = b.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(i * s + (i - 1) * bw + bw / 2, rt.anchoredPosition.y);

            int copy = i;
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(
                () =>
                {
                    var analyticEvent = new AnalyticEvent(AnalyticEventType.QuizResponse, this.AwsMobileAnalytics.AnalyticsManager);
                    analyticEvent.AddAttribute("ID", this.quiz.ID);
                    analyticEvent.AddMetric("Response", copy);
                    analyticEvent.Submit();

                    if (this.quiz.Questions[this.currentQuestion].Answer == copy)
                    {
                        StartCoroutine(OnCorrectResponse());
                    }
                    else
                    {
                        StartCoroutine(OnIncorrectResponse());
                    }
                });
            t = this.instantiatedQuiz.transform.Find("Option" + i).GetChild(0).gameObject.GetComponent<Text>();
            t.text = q.Options[i - 1].Value;
            t.color = ColorUtils.HexToColor(q.Options[i - 1].Color);
        }
    }

    // ------------------------------------------------------------------------------------- //

    private IEnumerator OnCorrectResponse()
    {
        this.instantiatedQuiz.transform.Find("CorrectPanel").gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        this.instantiatedQuiz.transform.Find("CorrectPanel").gameObject.SetActive(false);
        if (this.currentQuestion == this.quiz.Questions.Length - 1)
        {
            OnQuizCompleted();
        }
        else
        {
            LoadNextQuestion();
        }
    }

    // ------------------------------------------------------------------------------------- //

    private IEnumerator OnIncorrectResponse()
    {
        this.instantiatedQuiz.transform.Find("IncorrectPanel").gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        this.instantiatedQuiz.transform.Find("IncorrectPanel").gameObject.SetActive(false);
    }

    // ------------------------------------------------------------------------------------- //

    private void OnQuizCompleted()
    {
        GameObject.Destroy(this.instantiatedQuiz.gameObject);

        if (this.FirstPersonController != null &&
            this.quiz.CameraLocationX != 0 &&
            this.quiz.CameraLocationY != 0 &&
            this.quiz.CameraLocationZ != 0)
        {
            Debug.Log("Resetting fps controller");
            this.FirstPersonController.transform.position = this.originalCameraPosition;
            this.FirstPersonController.GetComponentInChildren<Camera>().cullingMask = this.orignalCullingMask;
            this.FirstPersonController.GetComponent<FirstPersonController>().enabled = true;
        }
    }

    // ------------------------------------------------------------------------------------- //

    public void Start()
    { }

    // ------------------------------------------------------------------------------------- //

    public void Update()
    { }

    // ------------------------------------------------------------------------------------- //
}
