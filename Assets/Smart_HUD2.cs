using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
//using Amazon;



public class Smart_HUD2 : MonoBehaviour 
{

	[System.Serializable] 
	public class Slidelist
	{
		public bool[] ObjOnOff;
	}

	public Slidelist[] slidelists; // Creates an array of array of bools for setting which game object should appear on which slide


	[System.Serializable] 
	public class slideAudio
	{
		public AudioClip[] clipToPlay;
	}

	public slideAudio[] slideAudiolist = new slideAudio[3]; // devides the audioclips across difficulty  [0] = easy, [1] = medium, [2] = hard

	[System.Serializable] 
	public class slideAudioCheck
	{
		public bool[] wasAudioPlayed;
	}

	public slideAudioCheck[] slideAudioChecklist = new slideAudioCheck[3]; // slideAudioCheck keeps track of which audio has been played  [0] = easy, [1] = medium, [2] = hard


	private LessonJSON quiz;
	private int currentLesson = 0;
	private int upperBound;
	private bool cursorVisible;
	private GameObject FindFPSC;
	private GameObject FindFPCCam;
	private bool hardlesson_displayed;
	private int zeta = 0;
	private int lessondifficulty = 0;
	private  bool[] audioplayed; //has the audio file been played (once)
	private DateTime nextTriggerTime;
	private int LessonSlide; //LessonSlide = What lesson are you on? The big lesson
	private AudioSource music;
	private AudioClip clipBeingPlayed;
	private PauseMenu pauseMenu;

	//    public AudioClip[] slideaudio;

	public Button replayaudio;
	public AudioSource audiosource;
	public Font Font;
	public GameObject[] slideObjects;
	public Camera cam;
	public Button btn1;
	public Button btn2;
	public Button btn3;
	public Button btn4;
	public Text Text1;
	//  public Button levelbutton;
	public TextAsset QuizToLoad;
	public HeliumAtom HeAtom;
	public GameObject ButtonAppear;
	public InterfaceAnimManager IAM;
	public GameObject Halo;
	public Spontaneous_emission sponEm;
	public bool Lesson0;
	public bool Lesson1;
	public bool Lesson2;
	public bool Lesson3;
	public bool Lesson4;
	public bool Lesson5;
	public bool Lesson6;
	public HeNe_controller HeNeScript;
	public Energy3LevelController EnergyL3controller;
	public Text GreenOrbText;
	public GameObject OrbHUD;
	public Dropdown dropMenu;
	public float duration_vol = 1.5f;
	public float smoothness_vol = 0.2f;
	public GameObject partsPush;
	public Animator anim_next;
	public Animator anim_exit;
	public GameObject LaserLesson0;
	public FirstPersonController FPControl;
	public Camera FPCamera;
	public GameObject ButtonLaunch;



	int advance = Animator.StringToHash("Advance");


	public void Update()
	{


		if (cursorVisible == true)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		//for lesson 4
		if (Lesson4 == true && currentLesson == 2 || Lesson4 == true && currentLesson == 4 )
		{
			HeNeScript.shouldPhotonSpawn = false;

		}

		if (Lesson4 == true && currentLesson == 3)
		{
			HeNeScript.shouldPhotonSpawn = true;

		}



	}


	// ------------------------------------------------------------------------------------- //


	public void Start()
	{
		cursorVisible = true;


		//        UnityInitializer.AttachToGameObject(this.gameObject);

		for (int i=0; i < slideAudioChecklist.Length; i++)
		{
			slideAudioChecklist[i].wasAudioPlayed = new bool[slidelists.Length];

		}



		//		music = GameObject.Find ("Music").GetComponent<AudioSource> ();

		Button b1 = btn1.GetComponent<Button>();
		Button b2 = btn2.GetComponent<Button>();
		Button b3 = btn3.GetComponent<Button>();
		Button b4 = btn4.GetComponent<Button>();

		//        Button level_button = levelbutton.GetComponent<Button>();
		//        level_button.onClick.AddListener(LevelChooser);


		//		b1.onClick.RemoveAllListeners();
		//		b2.onClick.RemoveAllListeners();
		b1.onClick.AddListener(LoadLastQuestion);
		b2.onClick.AddListener(NextQuestion);	
		b3.onClick.AddListener(ExitButtonClick);
		//		b4.onClick.AddListener(Excite);

		//		GameObject FindFPSC = GameObject.Find("FPSController");
		//		FPControl = FindFPSC.gameObject.GetComponent<FirstPersonController>();
		//
		//		GameObject FindFPCCam = GameObject.Find("FirstPersonCharacter");
		//		FPCamera = FindFPCCam.gameObject.GetComponent<Camera>();

		//		pauseMenu = GameObject.Find ("PauseMenu").GetComponent<PauseMenu> ();

		if (Lesson0 == true)
		{
			this.LoadCamera (this.QuizToLoad);
			this.IAM.startAppear ();
		}


	}

	// ------------------------------------------------------------------------------------- //

	public void LessonStart()
	{
		this.LoadCamera (this.QuizToLoad);
		this.IAM.startAppear ();
		ButtonLaunch.SetActive (false);
	}


	public void LevelChooser()
	{

		//        zeta++;
		//        int level = zeta % 3;
		//        Debug.Log("Zeta = " + zeta);
		//        Debug.Log("level =" + level);
		//
		if (dropMenu.value == 0)
		{
			//			levelbutton.GetComponentInChildren<Text>().text = "Beginner";
			lessondifficulty = 0;
		} 
		else if (dropMenu.value == 1)
		{
			//			levelbutton.GetComponentInChildren<Text>().text = "Intermediate";
			lessondifficulty = 1;
		}
		else if (dropMenu.value == 2)
		{
			//			levelbutton.GetComponentInChildren<Text>().text = "Advanced";
			lessondifficulty = 2;
		}


		//        if (level == 0)
		//        {
		//            levelbutton.GetComponentInChildren<Text>().text = "Beginner";
		//            lessondifficulty = 0;
		//        } 
		//        else if (level == 1)
		//        {
		//            levelbutton.GetComponentInChildren<Text>().text = "Intermediate";
		//            lessondifficulty = 1;
		//        }
		//        else if (level == 2)
		//        {
		//            levelbutton.GetComponentInChildren<Text>().text = "Advanced";
		//            lessondifficulty = 2;
		//        }

		DisplayText();
		Soundcheck();
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
			//            this.HeAtom.InLesson = true;
		}

	}

	// ------------------------------------------------------------------------------------- //

	public void LoadCamera(TextAsset quizToLoad)
	{
		this.Halo.SetActive(false);
		quiz = JsonUtility.FromJson<LessonJSON>(quizToLoad.text);

		//		this.cam.enabled = true;
		//this.FPC_cam.enabled = false;
		//        this.FPCamera.enabled = false;
		//this.FirstPersonController.GetComponent<FirstPersonController>().enabled = false;
		//        this.FPControl.GetComponent<FirstPersonController>().enabled = false;
		cursorVisible = true;
		//		OrbHUD.SetActive (false);
		//		pauseMenu.pauseMenuAccessible = false;
		//		StartCoroutine(VolumeFade_down (0.2f));

		upperBound = this.quiz.EasyLesson.Length - 1; //number of slides - 1 for arrays starting at zero
		LessonSlide = quiz.LessonID; 


		Text1.GetComponent<Text>().enabled = true;
		//        audioplayed = new bool[upperBound+1];
		//        for (int j = 0; j < upperBound; j++){
		//            this.audioplayed[j] = false;
		//        }
		if (currentLesson == 0) { StartCoroutine(DelayforHUD()); }

		LoadNextQuestion();
	}

	// ------------------------------------------------------------------------------------- //


	public void LoadNextQuestion() 
	{


		for (int j = 0; j < slidelists[currentLesson].ObjOnOff.Length; j++)
		{

			if (slideObjects[j] == null)
			{
				return;
			}

			if (slidelists[currentLesson].ObjOnOff[j] == true)
			{
				slideObjects[j].SetActive(true);
			}
			else
			{
				slideObjects[j].SetActive(false);

			}

		}
		anim_next.SetBool (advance, false);

		DisplayText();

		Soundcheck();



	}

	// ------------------------------------------------------------------------------------- //
	void DisplayText()
	{
		//  Lessontxt y = this.quiz.LessonID[currentLesson];
		Lessontxt easylesson = this.quiz.EasyLesson[this.currentLesson];
		Lessontxt mediumlesson = this.quiz.MediumLesson[this.currentLesson];
		Lessontxt hardlesson = this.quiz.HardLesson[this.currentLesson];


		if (lessondifficulty == 0)
		{
			Text1.text = easylesson.Message;
			hardlesson_displayed = false;

		}
		else if (lessondifficulty == 1)
		{
			Text1.text = mediumlesson.Message; ;
			hardlesson_displayed = false;

		}

		else if (lessondifficulty == 2)
		{
			Text1.text = hardlesson.Message;
			hardlesson_displayed = true;
		}
		//  Text1.text = easylesson.Message;

	}
	// ------------------------------------------------------------------------------------- //

	void Soundcheck()
	{
		if (this.slideAudiolist[lessondifficulty].clipToPlay[currentLesson] == null)
		{
			return;
		}

		if (this.slideAudioChecklist[lessondifficulty].wasAudioPlayed[currentLesson] == false  && currentLesson > 0)
		{
			PlaySound(slideAudiolist[lessondifficulty].clipToPlay[currentLesson]);
			this.slideAudioChecklist[lessondifficulty].wasAudioPlayed[currentLesson] = true;
		}

		else if (slideAudioChecklist[lessondifficulty].wasAudioPlayed[currentLesson] == true)
		{
			return;
		}

		Button audioreplay = replayaudio.GetComponent<Button>();
		audioreplay.onClick.AddListener(delegate { PlaySound(slideAudiolist[lessondifficulty].clipToPlay[currentLesson]); });
	}



	// ------------------------------------------------------------------------------------- //
	public void NextQuestion()
	{
		if (this.currentLesson == upperBound)
		{
			return;
		}
		//            else if (slideaudio[currentLesson] != null)
		//        {
		//            StopSound();
		//        }

		this.currentLesson++;
		Debug.Log("LoadNextQuestion_complete currentlesson=" + currentLesson);
		LoadNextQuestion();
	}

	// ------------------------------------------------------------------------------------- //


	public void LoadLastQuestion()
	{
		if (this.currentLesson == 0)
		{
			return;
		}
		//        else if (slideaudio[currentLesson] != null)
		//        {
		//            StopSound();
		//        }
		this.currentLesson--;
		LoadNextQuestion();
		audiosource.Stop ();

	}

	// ------------------------------------------------------------------------------------- //

	public void PlaySound(AudioClip thatClip)
	{
		audiosource.Stop ();
		StopCoroutine (CheckAudioLength ());
		audiosource.clip = thatClip;
		clipBeingPlayed = thatClip;


		//		if (lessondifficulty == 0)
		//		{
		//			audiosource.clip = lessonAudio_Simple[playindex];
		//		}
		//		else if (lessondifficulty == 1)
		//		{
		//			audiosource.clip = lessonAudio_Int[playindex];
		//		}
		//
		//		else if (lessondifficulty == 2)
		//		{
		//			audiosource.clip = lessonAudio_Adv[playindex];
		//		}

		audiosource.Play();
		StartCoroutine (CheckAudioLength ());

	}

	// ------------------------------------------------------------------------------------- //

	public IEnumerator CheckAudioLength()
	{
		yield return new WaitForSeconds (clipBeingPlayed.length);

		if(currentLesson == upperBound)
		{
			anim_exit.SetBool (advance, true);

		}
		else{
			anim_next.SetBool (advance, true);

		}


	}

	// ------------------------------------------------------------------------------------- //

	public void StopSound()
	{

		audiosource.Stop();

	}

	// ------------------------------------------------------------------------------------- //

	public void ExitButtonClick()
	{

//		if (Lesson4 == true)  //spon em of lesson 4 needs to be accessed before the gameObject is set to be inactive.  Thus this code needs to be up here instead of neatly tucked below :( grumble grumble
//		{
//			GreenOrbText.text = "4 / 6";
//			Spontaneous_emission se = GameObject.Find ("NeAtom_Lesson4").GetComponent<Spontaneous_emission> ();
//			se.DestroyPhoton ();
//			//            GameProgress.MarkCheckpointComplete(Checkpoint.Lesson4Complete);
//		}

		StopSound();
		//        this.cam.enabled = false;
		//this.FPC_cam.enabled = true;
		//        this.FPCamera.enabled = true;
		//        this.FPControl.GetComponent<FirstPersonController>().enabled = true;
		//        // this.FirstPersonController.GetComponent<FirstPersonController>().enabled = true;
		Text1.GetComponent<Text>().enabled = false;
		for (int j = 0; j < slideObjects.Length; j++)
		{
			if (slideObjects[j] != null)
			{
				slideObjects[j].SetActive(false);
			}

		}

		for (int i = 0; i < slideAudioChecklist.Length; i++) {
			for (int j = 0; j < slidelists.Length; j++) {
				slideAudioChecklist [i].wasAudioPlayed [j] = false;
			}
		}

		currentLesson = 0;
		//		this.Halo.SetActive(true);
		this.IAM.startDisappear ();
		ButtonAppear.SetActive(false);
		ButtonLaunch.SetActive (true);

		//        cursorVisible = false;
		//		OrbHUD.SetActive (true);
		//		StartCoroutine(VolumeFade_up (1f));
		//		pauseMenu.pauseMenuAccessible = true;
		anim_exit.SetBool (advance, false);


//		if(Lesson0 == true)
//		{
//			this.Halo.SetActive (false);
//		}
//
//		if (Lesson1 == true)
//		{
//			//			GreenOrbText.text = "1 / 6";
//			//            GameProgress.MarkCheckpointComplete(Checkpoint.Lesson1Complete);
//		}
//
//		if (Lesson2 == true)
//		{
//			//GreenOrbText.text = "2 / 6";
//			//            GameProgress.MarkCheckpointComplete(Checkpoint.Lesson2Complete);
//		}
//
//		if (Lesson3 == true)
//		{
//			GreenOrbText.text = "3 / 6";
//			wallMove wm = GameObject.Find ("LaserMiniGameWall").GetComponent<wallMove> ();
//			wm.openWall = true;
//			//            GameProgress.MarkCheckpointComplete(Checkpoint.Lesson3Complete);
//		}





//		if (Lesson5 == true)
//		{
//			//GreenOrbText.text = "5 / 6";
//			//            GameProgress.MarkCheckpointComplete(Checkpoint.Lesson5Complete);
//
//			//partsPush.SetActive (true);
//
//			//Transform allChildren = partsPush.GetComponentInChildren<Transform> ();
//			foreach(Transform child in allChildren)
//			{
//				child.gameObject.GetComponent<Rigidbody> ().angularVelocity = new Vector3 (0.1f, 0.1f, 0.1f);
//			}
//
//
//		}

//		if (Lesson6 == true)
//		{
//			GreenOrbText.text = "6 / 6";
//			//            GameProgress.MarkCheckpointComplete(Checkpoint.Lesson6Complete);
//			wallMove wm = GameObject.Find ("DnDGameWall").GetComponent<wallMove> ();
//			wm.openWall = true;
//
//		}

	}

	// ------------------------------------------------------------------------------------- //

	private IEnumerator DelayforHUD()
	{
		yield return new WaitForSeconds(1);
		PlaySound (slideAudiolist [lessondifficulty].clipToPlay [currentLesson]);
		this.slideAudioChecklist [lessondifficulty].wasAudioPlayed [currentLesson] = true;

	}

	// ------------------------------------------------------------------------------------- //

	public void Excite()
	{
		if (Lesson1 == true || Lesson2 == true) {
			HeAtom.Excite();
		}

		if (Lesson3 == true) {
			StartCoroutine (Lesson3StimEm ());
		}

		if (Lesson4 == true  && currentLesson <= 3) {
			HeNeScript.StartHeNeEnergyTransfer ();
		}

		if (Lesson4 == true  && currentLesson == 4) {
			EnergyL3controller.Excite ();
		}

		if (Lesson5 == true) {
			StartCoroutine (Lesson5StimEm ());
		}


	}

	// ------------------------------------------------------------------------------------- //

	public IEnumerator Lesson3StimEm()
	{
		HeAtom.Excite();
		yield return new WaitForSeconds (.6f);
		sponEm.SpawnPhoton_sponEm ();
		yield return new WaitForSeconds (1.3f);
		sponEm.SpawnPhoton_double ();

		yield break;

	}

	// ------------------------------------------------------------------------------------- //

	public IEnumerator Lesson5StimEm()
	{
		HeNeScript.NeExcitation ();
		yield return new WaitForSeconds (1.2f);
		sponEm.SpawnPhoton_sponEm ();
		yield return new WaitForSeconds (1.3f);
		sponEm.SpawnPhoton_double ();
		HeNeScript.NeReset ();

		yield break;
	}

	// ------------------------------------------------------------------------------------- //

	//	public IEnumerator VolumeFade_down(float volumeSet)
	//	{
	//		float increment_vol = smoothness_vol/duration_vol; //The amount of change to apply.
	//		while(music.volume > volumeSet)
	//		{
	//			music.volume -= increment_vol;
	//			yield return new WaitForSeconds(smoothness_vol);
	//		}
	//		yield break ;
	//	}
	//
	//	// ------------------------------------------------------------------------------------- //
	//
	//	public IEnumerator VolumeFade_up(float volumeSet)
	//	{
	//		float increment_vol = smoothness_vol/duration_vol; //The amount of change to apply.
	//		while(music.volume < volumeSet)
	//		{
	//			music.volume += increment_vol;
	//			yield return new WaitForSeconds(smoothness_vol);
	//		}
	//		yield break ;
	//	}
}