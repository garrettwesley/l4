using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;



public class SmartHUD : MonoBehaviour
{


    private LessonJSON quiz;
    private Vector3 originalCameraPosition;
    private int currentLesson;
    private int br;

    public Font Font;
    public GameObject AtomStatic;
    public GameObject AtomAnim;
    public FirstPersonController FirstPersonController;
	public Camera cam;
	public Camera FPC_cam;
    public Button yourButton;
    public Button yourButton2;
    public Button yourButton4;
	public Text Text1;
	private DateTime nextTriggerTime;

	public TextAsset QuizToLoad;



	void Start () {
		cam.enabled = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
//				if (DateTime.Now < this.nextTriggerTime)
//				{
//					return;
//				}
			Debug.Log ("yoyoyoyoy");
			this.LoadCamera(this.QuizToLoad);
//			this.nextTriggerTime = DateTime.Now + TimeSpan.FromSeconds(10);
			}



		}


    public void LoadCamera(TextAsset quizToLoad)
    {
//		GUI.enabled = true;
        this.quiz = JsonUtility.FromJson<LessonJSON>(quizToLoad.text);
		this.cam.enabled = true;
		this.FPC_cam.enabled = false;
		this.FirstPersonController.GetComponent<FirstPersonController>().enabled = false;
		Cursor.visible = true;
       
//        if (this.FirstPersonController != null &&
//            this.quiz.CameraLocationX != 0 &&
//            this.quiz.CameraLocationY != 0 &&
//            this.quiz.CameraLocationZ != 0)
//        {
//            this.originalCameraPosition = this.FirstPersonController.transform.position;
//            Debug.Log(string.Format("Setting fps position to {0}", new Vector3(this.quiz.CameraLocationX, this.quiz.CameraLocationY, this.quiz.CameraLocationZ)));
//            this.FirstPersonController.transform.position = new Vector3(
//                this.quiz.CameraLocationX,
//                this.quiz.CameraLocationY,
//                this.quiz.CameraLocationZ);
//            Camera c = this.FirstPersonController.GetComponentInChildren<Camera>();
//            c.gameObject.transform.LookAt(
//                new Vector3(
//                    this.quiz.CameraLookAtX,
//                    this.quiz.CameraLookAtY,
//                    this.quiz.CameraLookAtZ));
//			this.cam.enabled = true;
//            this.FirstPersonController.GetComponent<FirstPersonController>().enabled = false;
//
//        }
        AtomStatic.SetActive(true);
        AtomAnim.SetActive(false);

        this.currentLesson = 0;
 //       br = this.quiz.Lessons.Length - 1;
        Debug.Log("starting i=" + currentLesson);
        LoadNextQuestion();
    }

    // ------------------------------------------------------------------------------------- //
	public void LoadNextQuestion() {
        Button b = yourButton.GetComponent<Button>();
        Button b2 = yourButton2.GetComponent<Button>();
        Button b4 = yourButton4.GetComponent<Button>();




        b.onClick.RemoveAllListeners();
        b2.onClick.RemoveAllListeners();
        if (this.currentLesson > br) { this.currentLesson = br; }
        if (this.currentLesson <= 1 )
        {
            AtomStatic.SetActive(true);
            AtomAnim.SetActive(false);
        }
        else if (this.currentLesson > 1)
        {
            AtomStatic.SetActive(false);
            AtomAnim.SetActive(true);
        }
    //    Lessontxt z = this.quiz.Lessons[this.currentLesson];
		Text1.GetComponent<Text>().enabled = true;
		//Text1.text = z.Message;
       

        Debug.Log("LoadNextQuestion i=" + currentLesson);

		b.onClick.AddListener(NextQuestion);
		b2.onClick.AddListener(LoadLastQuestion);
		b4.onClick.AddListener(ExitButtonClick);	
		Debug.Log("hey_i'mworking");
    }


	public void NextQuestion()
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


	public void LoadLastQuestion()
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

	public void ExitButtonClick()
    {
        if (this.FirstPersonController != null &&
           this.quiz.CameraLocationX != 0 &&
           this.quiz.CameraLocationY != 0 &&
           this.quiz.CameraLocationZ != 0)
        {
            Debug.Log("Resetting fps controller");
//            this.FirstPersonController.transform.position = this.originalCameraPosition;
            this.FirstPersonController.GetComponent<FirstPersonController>().enabled = true;
            Text1.GetComponent<Text>().enabled = false;
            AtomStatic.SetActive(false);
            AtomAnim.SetActive(false);

        }
    }


}


