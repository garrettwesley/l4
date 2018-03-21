// Copyright (C) 2017 Lux Science, Inc. - All Rights Reserved


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MarsIntroController : MonoBehaviour {

	private Vector3 shuttlePosStart = new Vector3 (374.3f, 164.4f, 106.6f);
	private bool isSkipMenuOpen = false;

	//intro cameras
	public Camera outerCam;
	public Camera helmCam;
	public Camera cam2;
	public Camera suitCam;
	public Transform helmLookPosition1;
	public Transform AstPos1;
	public Transform AstPos2;
	public GameObject Astronaut;
	public GameObject menu;
	public AudioSource backgroundAudio;  //calm music in scene
	public AudioSource speechAudio;
	public AudioSource DDOL_music;  // calm music attached to Do Not Destroy on Load GM that will continue playing into next scene of exit.
	public Animator anim_menu;
	public Animator anim_shuttle;
	public Animator anim_end;
	public Animator anim_camPan;
	public Material visor_glass_mat;
	public Material visor_upper_mat;
	public GameObject visor_glass; 
	public GameObject visor_upper;
	public GameObject LuxScreen;
	public GameObject menuCanvas;
	public GameObject shuttle;
	public GameObject station;
	public Light dirLight;
	public GameObject PayUs;
	public GameObject credits;
	public GameObject skipMenu;
	public GameObject areYouSureSkip;


	//end sequence cameras
	public Camera flyByCam;
	public Camera dockingCam;
	public Camera shipCam;
	public Camera stationCam;





	public float duration_vol;
	public float smoothness_vol;
	private bool cursorVisible;


//	MarsIntroRotation mir;
	MasterControlScript mcs;

	int menufade = Animator.StringToHash("MenuFade");
	int blackbox = Animator.StringToHash("BlackBox_on");
//	int blackbox_trans = Animator.StringToHash("BlackBox_transition");
	int luxOn = Animator.StringToHash("LuxOn");
	int flyby = Animator.StringToHash ("FlyBy");
	int camPan = Animator.StringToHash("IntroCamPan");

	void Start () {

//		mir = GameObject.Find("Astronaut").GetComponent<MarsIntroRotation>();
		mcs = GameObject.Find ("DontDestroyonLoad").GetComponent<MasterControlScript> ();


		if (mcs.exitSequece == false)
		{
			station.gameObject.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
			shuttle.SetActive (false);
			backgroundAudio.Play ();
			StartCoroutine (IntroScreen ());

		}

		else if (mcs.exitSequece == true)
		{
			DDOL_music.Play ();
			station.gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);
			shuttle.SetActive (true);
			menuCanvas.SetActive (false);
			outerCam.enabled = false;
			Astronaut.SetActive (false);
			dirLight.intensity = 1;
			mcs.gameObject.GetComponentInChildren<AudioSource> ().Play ();
			StartCoroutine (ExitSequence ());

		}

		if(mcs.gameCompleted == true)
		{
			PayUs.SetActive (true);
			cursorVisible = true;
		}
	}



	void Update () {

		if (Input.GetKeyDown(KeyCode.B))
		{

			if (isSkipMenuOpen == false)
			{
				areYouSureSkip.SetActive (true);

				StartCoroutine(OpenSkipMenu ());
				print ("Test");
			}

			if (isSkipMenuOpen == true)
			{
				StartCoroutine(CloseSkipMenu ());
			}

		}

		if (cursorVisible == true)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}



		
	}



	public IEnumerator OpenSkipMenu()
	{
		areYouSureSkip.SetActive (true);
		cursorVisible = true;
		print ("Test1");
		yield return new WaitForSeconds (0.1f);

		isSkipMenuOpen = true;
		yield break;

	}

	public IEnumerator CloseSkipMenu()
	{
		areYouSureSkip.SetActive (false);
		cursorVisible = false;
		yield return new WaitForSeconds (0.1f);
		isSkipMenuOpen = false;
		yield break;



	}



	public IEnumerator IntroScreen()
	{
		yield return new WaitForSeconds (3f);
		anim_menu.SetBool (luxOn, true);

		yield return new WaitForSeconds (2f);
		LuxScreen.SetActive (false);
//		anim_menu.SetBool (blackbox_trans, true);

		
	}

	public IEnumerator IntroSequence()
	{
		yield return new WaitForSeconds (3);
		menu.SetActive (false);
		anim_menu.SetBool (menufade, false);
		anim_camPan.SetBool (camPan, true);


//		menu.GetComponent<CanvasGroup> ().alpha = 0;

		yield return new WaitForSeconds (5);

		StartCoroutine (VolumeFade_down (0.2f));
		speechAudio.Play ();



		yield return new WaitForSeconds (3f);

		skipMenu.SetActive (true);

		yield return new WaitForSeconds (11f);



		//Space Suit Shot
		Astronaut.gameObject.transform.position = AstPos1.position;
		helmCam.enabled = false;
		outerCam.enabled = false;
		cam2.enabled = false;
		suitCam.enabled = true;


		yield return new WaitForSeconds (14f);


		//Helmet shot
		Vector3 helmPos = helmLookPosition1.position;
		Astronaut.gameObject.transform.eulerAngles = helmPos;
		Astronaut.gameObject.transform.localScale = new Vector3 (0.9f,0.9f,0.9f);
		helmCam.enabled = true;
		outerCam.enabled = false;
		cam2.enabled = false;
		suitCam.enabled = false;
		visor_glass.GetComponent<Renderer> ().material = visor_glass_mat;
		visor_upper.GetComponent<Renderer> ().material = visor_upper_mat;


		yield return new WaitForSeconds (10f);


		//Atmosphere shot
		Astronaut.gameObject.transform.position = AstPos1.position;
		Astronaut.gameObject.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);
		helmCam.enabled = false;
		outerCam.enabled = false;
		cam2.enabled = true;
		suitCam.enabled = false;



		yield return new WaitForSeconds (10f);


		anim_menu.SetBool (blackbox, true);
		yield return new WaitForSeconds (5f);

		SceneManager.LoadScene ("spaceship_master2");

//		//Outer cam shot
//		Astronaut.gameObject.transform.position = AstPos2.position;
//		helmCam.enabled = false;
//		outerCam.enabled = true;
//		cam2.enabled = false;
//		suitCam.enabled = false;


	}


	public IEnumerator SkipIntro()
	{
		skipMenu.SetActive (false);
		speechAudio.Stop ();
		anim_menu.SetBool (blackbox, true);
		yield return new WaitForSeconds (5f);

		SceneManager.LoadScene ("spaceship_master2");

	}

	public void TriggerSkip()
	{
		StartCoroutine (SkipIntro ());
	}

	public void CancelSkip()
	{
		CloseSkipMenu ();
	}

	public void StartGame()
	{
		anim_menu.SetBool (menufade, true);
		StartCoroutine (IntroSequence ());
		cursorVisible = false;
		mcs.gameCompleted = false;

	}

	public void QuitGame()
	{
		Application.Quit ();
	}


	public IEnumerator VolumeFade_down(float volumeSet)
	{
		float increment_vol = smoothness_vol/duration_vol; //The amount of change to apply.
		while(backgroundAudio.volume > volumeSet)
		{
			backgroundAudio.volume -= increment_vol;
			yield return new WaitForSeconds(smoothness_vol);
		}
		yield break ;
	}

	public IEnumerator VolumeFade_up(float volumeSet)
	{
		float increment_vol = smoothness_vol/duration_vol; //The amount of change to apply.
		while(backgroundAudio.volume < volumeSet)
		{
			backgroundAudio.volume += increment_vol;
			yield return new WaitForSeconds(smoothness_vol);
		}
		yield break ;
	}


	public IEnumerator ExitSequence()

	{
		//ShipCam Sequence
		anim_shuttle.SetBool (flyby, true);
		shipCam.enabled = true;
		flyByCam.enabled = false;
		stationCam.enabled = false;
		dockingCam.enabled = false;

		yield return new WaitForSeconds (5f);

		//FlyBy shot
		shipCam.enabled = false;
		flyByCam.enabled = true;
		stationCam.enabled = false;
		dockingCam.enabled = false;

		yield return new WaitForSeconds (6f);

		//station shot
		shipCam.enabled = false;
		flyByCam.enabled = false;
		stationCam.enabled = true;
		dockingCam.enabled = false;

		yield return new WaitForSeconds (8f);

//		//station shot
//		shipCam.enabled = false;
//		flyByCam.enabled = false;
//		stationCam.enabled = false;
//		dockingCam.enabled = true;
//		anim_shuttle.SetBool (docking, true);

//		yield return new WaitForSeconds (2f);

		anim_end.SetBool (blackbox, true);

		yield return new WaitForSeconds (3f);

		mcs.exitSequece = false;
		mcs.finalShipSequence = true;
		backgroundAudio.Stop ();
		SceneManager.LoadScene ("spaceship_master2");


		yield break;
	}

	public void Donate()
	{
		Application.OpenURL ("http://www.luxscience.com");

	}

	public void Survey()
	{
		Application.OpenURL ("http://www.luxscience.com/episode1_survey.html");

	}

	public void Credits()
	{
		if( credits.active == false)
		{
			credits.SetActive (true);
		}
		else
		{
			credits.SetActive (false);

		}
		
	}

}
