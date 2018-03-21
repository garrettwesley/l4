using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;



public class BridgeController : MonoBehaviour {

	private GameObject bridgeDoor;  //creating a varaible for the door that I want to open
	private GameObject crewDoor1;
	private GameObject crewDoor2;
	private GameObject ddol;
	private float duration = 1f; // Duration of flashing red lights during red alert
	private float smoothness = 0.02f; 
	private bool isSkipMenuOpen = false;
	private bool cursorVisible;
	private bool skipMenuDone = false;



	public float duration_vol;
	public float smoothness_vol;

	public GameObject BridgeScreen;
	public bool RedAlertActive;
	public bool BridgeIntroSequence = true;
	public Sprite[] screenImages;
	public Image image;
	public ObjectivesUI ui;
	public Light[] lights;
	public Color normal = new Color32 (178, 198, 255, 255);
	public Color RedAlert = new Color32 (255, 41, 41, 255);
	public Animator anim_fadeScreen;
	public Animator anim_fadeblack_final;
	public Animator anim_lock_exit;
	public Animator anim_ring_exit;
	public Animator finalCam;
	public Animator anim_shipMainDoorOpen;
	public Animator luxFade;
	public Camera camFPC;
	public Camera camScreen;
	public FirstPersonController FPC;
	public Animator anim_cam;
	public Animator anim_Bridgescreen;
	public AudioSource Scott_intro;
	public AudioSource mainMusic;
	public AudioSource intercom;
	public AudioClip redAlertMusic_clip;
	public GameObject checkEngineScreen;
	public GameObject lifeSupportScreen;
	public GameObject introScreen;
	public GameObject exit_GreenLock;
	public GameObject exit_BlueLock;
	public GameObject objCanvas;
	public GameObject spaceStation;
	public AudioClip[] intercomClip = new AudioClip[6];
	public GameObject O2guage;
	public GameObject O2guageScreen;
	public GameObject skipMenu;
	public GameObject areYouSureSkip;


	int blackbox = Animator.StringToHash("BlackBox_fadeout");
	int camTrigger = Animator.StringToHash("BridgeScreen");
	int skull = Animator.StringToHash("skull");
	int checkEngine = Animator.StringToHash("checkEngine");
	int startCam = Animator.StringToHash("startCam");
	int ring_disappear = Animator.StringToHash("ring_disappear");
	int lock_disappear = Animator.StringToHash("lock_disappear");
	int Open_door = Animator.StringToHash("Open_door");
	int openMainDoor = Animator.StringToHash("OpenMainDoor");
	int change2Green = Animator.StringToHash("change2Green");
	int luxOn = Animator.StringToHash("LuxOn");


	Door_Master[] door = new Door_Master[6];   //creating a variable for the script Door_Master
	MasterControlScript MCS;
	Tricorder_Trigger triTrig;
	leftDoorSlide holoDeckDoor;
	Door_Master holoDeckLock;

	//------------------------------------------------------------------------//

	void Awake(){


	}


	void Start () {



		GameObject bridgeDoor = GameObject.Find("Door_Bridge"); // set variable to actural game object without needing to drag from editor.
		GameObject crewDoor1 = GameObject.Find("Door_Crew1");
		GameObject crewDoor2 = GameObject.Find("Door_Crew2");
		GameObject labDoor1 = GameObject.Find("Door_Lab1");
		GameObject labDoor2 = GameObject.Find("Door_Lab2");
		GameObject engineDoor = GameObject.Find ("Door_Engine");
		door[0] = bridgeDoor.GetComponent<Door_Master> ();  //assign door to component Door_Master on labDoor1
		door[1] = crewDoor1.GetComponent<Door_Master> ();
		door[2] = crewDoor2.GetComponent<Door_Master> ();
		door[3] = labDoor1.GetComponent<Door_Master> ();
		door[4] = labDoor2.GetComponent<Door_Master> ();
		door[5] = engineDoor.GetComponent<Door_Master> ();

		triTrig = GameObject.Find ("Tricorder").GetComponent<Tricorder_Trigger> ();
		holoDeckDoor = GameObject.Find ("leftDoor").GetComponent<leftDoorSlide> ();
		holoDeckLock = GameObject.Find ("HolodeckLock").GetComponent<Door_Master> ();

	
		GameObject ddol = GameObject.Find("DontDestroyonLoad");
		MCS = ddol.gameObject.GetComponent<MasterControlScript>();

		holoDeckLock.unlockDoor ();

		if( MCS.firstTimeInShip == true)
		{
			anim_fadeScreen.SetBool (blackbox, true);
			StartCoroutine (LevelStart ());
		}

		else{
			introScreen.SetActive (false);
		}



		if(MCS.backInDaShip == true){
			BridgeIntroSequence = false;
			MCS.inHolodeck = true;
		}


		if (MCS.finalShipSequence == true)
			
		{
			
			mainMusic.enabled = false;
			FPC.enabled = false;
			camFPC.enabled = false;
			finalCam.SetBool (startCam, true);
			finalCam.gameObject.GetComponent<Camera> ().enabled = true;
			anim_fadeblack_final.gameObject.SetActive (true);
			luxFade.gameObject.SetActive (true);
			objCanvas.gameObject.SetActive (false);
			door[0].unlockDoor ();
			spaceStation.SetActive (true);
			O2guage.SetActive (false);
			StartCoroutine (EndSequence ());

		
		}


		if (MCS.reloadRedAlert == true)  // if O2 gauge goes to zero, O2Guage.cs sets reloadRedAlert = true and reloads the spaceshipScene
		{
			RedAlertSequence ();
			MCS.inCrew = true;
			door[0].unlockDoor ();
			door[1].unlockDoor ();
			door[2].unlockDoor ();
			door[3].unlockDoor ();
			door[4].unlockDoor ();
			holoDeckDoor.ActivateDoor (false);
			holoDeckLock.lockDoor ();
			mainMusic.clip = redAlertMusic_clip;
			mainMusic.Play ();
			O2guage.SetActive (true);
			O2guage.gameObject.GetComponent<O2Gauge> ().StartO2Counter ();
			RedAlertScreen ();


		}
	}

	//------------------------------------------------------------------------//
	
	void Update () 
	{
		if (skipMenuDone == false) {
			if (Input.GetKeyDown (KeyCode.B)) {

				if (isSkipMenuOpen == false) {
					areYouSureSkip.SetActive (true);

					StartCoroutine (OpenSkipMenu ());
					print ("Test");
				}

				if (isSkipMenuOpen == true) {
					StartCoroutine (CloseSkipMenu ());
				}

			}
		}

		if (cursorVisible == true)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}


		if (MCS.bridgeDoorUnlocked == true)
		{
			door[0].unlockDoor ();
		}

		if (MCS.crewDoorsUnlocked == true)
		{
			door[1].unlockDoor ();
			door[2].unlockDoor ();
		}

		if (MCS.labDoorsUnlocked == true)
		{
			door[3].unlockDoor ();
			door[4].unlockDoor ();
		}

		if(MCS.engineDoorUnlocked == true)
		{
			door [5].unlockDoor ();
		}


		if (RedAlertActive == true) 
		{
			for (int i = 0; i < lights.Length; i++) 
			{
				lights [i].color = RedAlert;
			}
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

	public IEnumerator SkipBridgeScene()
	{
		skipMenu.SetActive (false);
		Scott_intro.Stop ();
		StartCoroutine (VolumeFade_up (0.6f));
		FPC.enabled = true;
		camFPC.enabled = true;
		camScreen.enabled = false;
		ui.ActivateHUD (true);
		door[0].unlockDoor ();
		door[1].unlockDoor ();
		door[2].unlockDoor ();
		MCS.bridgeDoorUnlocked = true;
		MCS.crewDoorsUnlocked = true;
		ui.AdvanceObjective ();
		BridgeIntroSequence = false;
		StopCoroutine ("BridgeIntroScreenSequence");
		image.sprite = screenImages [7];
		skipMenu.transform.parent.gameObject.SetActive (false);
		skipMenuDone = true;


		yield return new WaitForSeconds (4f);

		intercom.clip = intercomClip [1];
		intercom.Play ();

		yield break;


	}

	public void TriggerSkip()
	{
		StartCoroutine (SkipBridgeScene ());
	}

	public void CancelSkip()
	{
		CloseSkipMenu ();
	}


	public IEnumerator LevelStart()
	{
		yield return new WaitForSeconds (5f);
		intercom.clip = intercomClip [0];
		intercom.Play ();

		yield return new WaitForSeconds (5f);
		ui.ShowUIonAdvance ();


		yield return new WaitForSeconds (5f);
		introScreen.SetActive (false);
		MCS.firstTimeInShip = false;
		BridgeIntroSequence = true;





	}

	public IEnumerator EndSequence ()
	{
		anim_fadeblack_final.SetBool (blackbox, true);

		yield return new WaitForSeconds (2f);

		door [0].anim_ring [0].SetBool (ring_disappear, true);
		door [0].anim_ring [1].SetBool (ring_disappear, true);

		door [0].anim_lock [0].SetBool (lock_disappear, true);
		door [0].anim_lock [1].SetBool (lock_disappear, true);

		door [0].doorsound.Play ();
		yield return new WaitForSeconds(1f);
		door [0].anim_door.SetBool (Open_door, true);

		yield return new WaitForSeconds (7f);
		exit_BlueLock.SetActive (false);
		exit_GreenLock.SetActive (true);
		anim_ring_exit.SetTrigger (change2Green);

		anim_ring_exit.SetBool (ring_disappear, true);
		anim_lock_exit.SetBool (lock_disappear, true);

		yield return new WaitForSeconds (1f);
		anim_shipMainDoorOpen.SetTrigger (openMainDoor);
		yield return new WaitForSeconds (5f);
		luxFade.SetBool (luxOn, true);
		yield return new WaitForSeconds (10f);
		MCS.gameCompleted = true;
		MCS.gameObject.GetComponentInChildren<AudioSource> ().Stop ();
		MCS.finalShipSequence = false;
		MCS.holoDeckLoaded = false;
		MCS.bridgeDoorUnlocked = false;
		MCS.crewDoorsUnlocked = false;
		MCS.labDoorsUnlocked = false;
		MCS.engineDoorUnlocked = false;
		MCS.badgePickedUp = false;
		MCS.wokeUp = false;
		MCS.firstTimeInShip = true;
		MCS.backInDaShip = false;
		MCS.reloadRedAlert = false;
		MCS.finalShipSequence = false;
		MCS.exitSequece = false;
		SceneManager.LoadScene ("IntroMarsOrbit");



	}


	//------------------------------------------------------------------------//

	public IEnumerator LerpColorUp()
	{
		print ("up");
		float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
		float increment = smoothness/duration; //The amount of change to apply.
		while(progress < 1)
		{
			RedAlert = Color.Lerp(Color.red, Color.black, progress);
			progress += increment;
			yield return new WaitForSeconds(smoothness);
		}
		StartCoroutine(LerpColorDown ());
		yield break ;
	}

	//------------------------------------------------------------------------//

	public IEnumerator LerpColorDown()
	{	
		print ("down");
		float progress = 1; //This float will serve as the 3rd parameter of the lerp function.
		float increment = smoothness/duration; //The amount of change to apply.
		while(progress > 0)
		{
			RedAlert = Color.Lerp(Color.red, Color.black, progress);
			progress -= increment;
			yield return new WaitForSeconds(smoothness);
		}
		StartCoroutine(LerpColorUp ());
		yield break ;
	}


	//------------------------------------------------------------------------//

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && BridgeIntroSequence == true)
		{
			FPC.enabled = false;
			camFPC.enabled = false;
			camScreen.enabled = true;
			anim_cam.SetBool (camTrigger, true);
			ui.ActivateHUD (false);
			StartCoroutine ("BridgeIntroScreenSequence");

		}

		if (other.tag == "Player"  && RedAlertActive == true)
		{
			door [5].unlockDoor ();
			MCS.engineDoorUnlocked = true;
			ui.AdvanceObjective ();
			intercom.clip = intercomClip [4];
			intercom.Play ();
			triTrig.ActivateTricorderPickup ();
			holoDeckDoor.ActivateDoor (false);
			holoDeckLock.lockDoor ();
			RedAlertActive = false;
			O2guage.SetActive (true);

		}
	}

	//------------------------------------------------------------------------//

	public void RedAlertSequence()
	{
		for(int i=0; i<lights.Length; i++)
		{
			lights [i].color = RedAlert;
			lights [i].intensity = 2f;
		}
//		door [3].lockDoor ();
//		door [4].lockDoor ();
		RedAlertActive = true;
		StartCoroutine (LerpColorUp ());
	}

	//------------------------------------------------------------------------//

	public void NormalLights()
	{
		for(int i=0; i<lights.Length; i++)
		{
			lights [i].color = normal;
			lights [i].intensity = 1.6f;
		}
	}

	//------------------------------------------------------------------------//

	public IEnumerator BridgeIntroScreenSequence()
	{
		yield return new WaitForSeconds (1f);
		StartCoroutine (VolumeFade_down (0.1f));
		yield return new WaitForSeconds (3f);

		Scott_intro.Play ();
		yield return new WaitForSeconds (.2f);

		int imgRef = 0;
		skipMenu.SetActive (true);
		image.sprite = screenImages [imgRef++]; // earth burn
		yield return new WaitForSeconds (5f); 
		image.sprite = screenImages [imgRef++]; // logo in space
		yield return new WaitForSeconds (5f);
		image.sprite = screenImages [imgRef++]; // earth + Mars  
		yield return new WaitForSeconds (5f);
		image.sprite = screenImages [imgRef++]; // jobs
		yield return new WaitForSeconds (4f);
		image.sprite = screenImages [imgRef++]; // resources
		yield return new WaitForSeconds (4f);
		anim_Bridgescreen.SetBool (skull, true);		//pirates
		yield return new WaitForSeconds (8f);

		image.sprite = screenImages [imgRef++];  // we need you



		yield return new WaitForSeconds (12f);
		image.sprite = screenImages [imgRef++];  // new day
		anim_Bridgescreen.SetBool (skull, false);		

		yield return new WaitForSeconds (6f); 
		image.sprite = screenImages [imgRef++];  // back to logo
		yield return new WaitForSeconds (6f);


		StartCoroutine (VolumeFade_up (0.6f));

		FPC.enabled = true;
		camFPC.enabled = true;
		camScreen.enabled = false;
		ui.ActivateHUD (true);
		door[0].unlockDoor ();
		door[1].unlockDoor ();
		door[2].unlockDoor ();
		MCS.bridgeDoorUnlocked = true;
		MCS.crewDoorsUnlocked = true;
		ui.AdvanceObjective ();
		BridgeIntroSequence = false;
		skipMenu.SetActive (false);


		yield return new WaitForSeconds (4f);

		intercom.clip = intercomClip [1];
		intercom.Play ();

		yield break;

	}


	public IEnumerator VolumeFade_down(float volumeSet)
	{
		float increment_vol = smoothness_vol/duration_vol; //The amount of change to apply.
		print ("test");
		while(mainMusic.volume > volumeSet)
		{
			mainMusic.volume -= increment_vol;
			yield return new WaitForSeconds(smoothness_vol);
		}
		yield break ;
	}

	public IEnumerator VolumeFade_up(float volumeSet)
	{
		float increment_vol = smoothness_vol/duration_vol; //The amount of change to apply.
		while(mainMusic.volume < volumeSet)
		{
			mainMusic.volume += increment_vol;
			yield return new WaitForSeconds(smoothness_vol);
		}
		yield break ;
	}

	public void RedAlertScreen()
	{
		checkEngineScreen.SetActive (true);
		lifeSupportScreen.SetActive (true);
		anim_Bridgescreen.SetBool (checkEngine, true);
		image.sprite = screenImages [8]; // boarder
		O2guageScreen.gameObject.GetComponent<O2Gauge> ().StartO2Counter ();



	}

}
