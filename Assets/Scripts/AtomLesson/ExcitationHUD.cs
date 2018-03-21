using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class ExcitationHUD : MonoBehaviour
{


    private LessonJSON quiz;
    private Vector3 originalCameraPosition;
    private int currentLesson;
    private int br;

    public Font Font;
    public GameObject Wave1;
    public GameObject Wave2;
    public GameObject AtomAnim;
    public FirstPersonController FirstPersonController;
    public Button yourButton;
    public Text Text1;
    public Button yourButton2;
    public Button yourButton4;


    public void LoadCamera(TextAsset quizToLoad)
    {
        this.quiz = JsonUtility.FromJson<LessonJSON>(quizToLoad.text);
       
       
        if (this.FirstPersonController != null &&
            this.quiz.CameraLocationX != 0 &&
            this.quiz.CameraLocationY != 0 &&
            this.quiz.CameraLocationZ != 0)
        {
            this.originalCameraPosition = this.FirstPersonController.transform.position;
            Debug.Log(string.Format("Setting fps position to {0}", new Vector3(this.quiz.CameraLocationX, this.quiz.CameraLocationY, this.quiz.CameraLocationZ)));
            this.FirstPersonController.transform.position = new Vector3(
                this.quiz.CameraLocationX,
                this.quiz.CameraLocationY,
                this.quiz.CameraLocationZ);
            Camera c = this.FirstPersonController.GetComponentInChildren<Camera>();
            c.gameObject.transform.LookAt(
                new Vector3(
                    this.quiz.CameraLookAtX,
                    this.quiz.CameraLookAtY,
                    this.quiz.CameraLookAtZ));
            this.FirstPersonController.GetComponent<FirstPersonController>().enabled = false;

        }
        Wave2.SetActive(false);
        Wave1.SetActive(false);
        AtomAnim.SetActive(true);

        this.currentLesson = 0;
       // br = this.quiz.Lessons.Length - 1;
        Debug.Log("starting i=" + currentLesson);
        LoadNextQuestion();
    }

    // ------------------------------------------------------------------------------------- //
    private void LoadNextQuestion() {
        Button b = yourButton.GetComponent<Button>();
        Button b2 = yourButton2.GetComponent<Button>();
        Button b4 = yourButton4.GetComponent<Button>();

        b.onClick.RemoveAllListeners();
        b2.onClick.RemoveAllListeners();
        if (this.currentLesson > br) { this.currentLesson = br; }
        if (this.currentLesson <= 2 )
        {
            Wave2.SetActive(false);
            Wave1.SetActive(false);
            AtomAnim.SetActive(true);
        }
        else if (this.currentLesson > 2)
        {
            Wave2.SetActive(true);
            Wave1.SetActive(true);
            AtomAnim.SetActive(false);
        }
        //Lessontxt z = this.quiz.Lessons[this.currentLesson];
    //    Text1.text = z.Message;
        Text1.GetComponent<Text>().enabled = true;

        Debug.Log("LoadNextQuestion i=" + currentLesson);

        b.onClick.AddListener(NextQuestion);
        b2.onClick.AddListener(LoadLastQuestion);
        b4.onClick.AddListener(ExitButtonClick);
    }

    private void NextQuestion()
    {
        if (this.currentLesson == br)
        {
            Debug.Log("Reached max");
            this.currentLesson--;
        }
        else if (this.currentLesson < br)
        {
            this.currentLesson++;
            Debug.Log("NExtQuestion i=" + currentLesson);
            LoadNextQuestion();
        }

    }


    private void LoadLastQuestion()
    {
        if (this.currentLesson == 0)
        {
            Debug.Log("Reached min");
            this.currentLesson = 0;
        }
        else if (this.currentLesson > 0)
        {
            this.currentLesson--;
            Debug.Log("LastQuestion i=" + currentLesson);
            LoadNextQuestion();
        }
     }

    private void ExitButtonClick()
    {
        if (this.FirstPersonController != null &&
           this.quiz.CameraLocationX != 0 &&
           this.quiz.CameraLocationY != 0 &&
           this.quiz.CameraLocationZ != 0)
        {
            Debug.Log("Resetting fps controller");
            this.FirstPersonController.transform.position = this.originalCameraPosition;
            this.FirstPersonController.GetComponent<FirstPersonController>().enabled = true;
            Text1.GetComponent<Text>().enabled = false;
            Wave2.SetActive(false);
            Wave1.SetActive(false);
            AtomAnim.SetActive(false);

        }
    }
}


