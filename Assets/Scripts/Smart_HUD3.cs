using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;



public class Smart_HUD3 : MonoBehaviour 
{
	[System.Serializable] 
	public class Slidelist
	{
		public bool[] slides;
	}
	public Slidelist[] slidelists; // Creates an array of array of bools for setting which game object should appear on which slide


	private LessonJSON quiz;
	private int currentLesson = 0;
	private int upperBound;
	private bool cursorVisible;


	public Font Font;
	public GameObject[] slideObjects;

	public FirstPersonController FirstPersonController;
	public Camera cam;
	public Camera FPC_cam;
	public Button btn1;
	public Button btn2;
	public Button btn3;
	public Button btn4;
	public Text Text1;
	private DateTime nextTriggerTime;
	public TextAsset QuizToLoad;
	public HeliumAtom HeAtom;
	public GameObject ButtonAppear;
	public InterfaceAnimManager IAM;
	public GameObject Halo;



	public void Update()
	{



			Cursor.visible = true;	
			Cursor.lockState = CursorLockMode.None;

	}


	// ------------------------------------------------------------------------------------- //


	public void Start()
	{

		Button b1 = btn1.GetComponent<Button>();
		Button b2 = btn2.GetComponent<Button>();
		Button b3 = btn3.GetComponent<Button>();
		Button b4 = btn4.GetComponent<Button>();
//		b1.onClick.RemoveAllListeners();
//		b2.onClick.RemoveAllListeners();
		b1.onClick.AddListener(LoadLastQuestion);
		b2.onClick.AddListener(NextQuestion);	
		b3.onClick.AddListener(ExitButtonClick);
		b4.onClick.AddListener(Excite);
	

	}




	// ------------------------------------------------------------------------------------- //

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			if (DateTime.Now < this.nextTriggerTime) {
				return;
			}
			this.LoadCamera (this.QuizToLoad);
			this.IAM.startAppear ();
			this.nextTriggerTime = DateTime.Now + TimeSpan.FromSeconds (5);
		}
	}

	// ------------------------------------------------------------------------------------- //

	public void LoadCamera(TextAsset quizToLoad)
	{
		this.Halo.SetActive(false);
		this.quiz = JsonUtility.FromJson<LessonJSON>(quizToLoad.text);
		this.cam.enabled = true;
		this.FPC_cam.enabled = false;
		this.FirstPersonController.GetComponent<FirstPersonController>().enabled = false;
		cursorVisible = true;	

//		slideObjects[0].SetActive(true);
//		slideObjects[1].SetActive(false);

//		upperBound = this.quiz.Lessons.Length - 1;
		Debug.Log("starting i=" + currentLesson);
		Text1.GetComponent<Text>().enabled = true;



		LoadNextQuestion();
	}

		// ------------------------------------------------------------------------------------- //


	public void LoadNextQuestion() 
	{


//		if(this.slides[0][currentLesson] == true)

		for (int i=0; i< slideObjects.Length; i++)
		{
			slideObjects[i].SetActive(slidelists[i].slides[currentLesson]);

		}

			


	//	Lessontxt z = this.quiz.Lessons[this.currentLesson];

	//	Text1.text = z.Message;


		Debug.Log("LoadNextQuestion i=" + currentLesson);

		if (ButtonAppear == null)
		{
			return;
			}
		else if (this.currentLesson >= 2)
		{
			ButtonAppear.SetActive(true);
		}
		else{ ButtonAppear.SetActive(false); }

	}

	// ------------------------------------------------------------------------------------- //


	public void NextQuestion()
		{
			if (this.currentLesson == upperBound)
			{
				return;
			}
			
		this.currentLesson++;
			Debug.Log("NExtQuestion i=" + currentLesson);
			LoadNextQuestion();
		}

	// ------------------------------------------------------------------------------------- //


	public void LoadLastQuestion()
	{
		if (this.currentLesson == 0)
		{
			return;
		}
		
		this.currentLesson--;
		LoadNextQuestion();
		
	}

	// ------------------------------------------------------------------------------------- //


	public void ExitButtonClick()
	{
		this.cam.enabled = false;
		this.FPC_cam.enabled = true;
		this.FirstPersonController.GetComponent<FirstPersonController>().enabled = true;
		Text1.GetComponent<Text>().enabled = false;
		slideObjects[0].SetActive(false);
		slideObjects[1].SetActive(false);
		currentLesson = 0;
		this.Halo.SetActive(true);
		this.IAM.startDisappear ();
		ButtonAppear.SetActive(false);

	}
		
	// ------------------------------------------------------------------------------------- //

	public void Excite()
	{
		HeAtom.StartLoop = true;

	}

	}


