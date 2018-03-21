using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EngineLaserController : MonoBehaviour {

	private bool controllerActive;
	private LaserPartsPickupController lppc;
	private Animator animLaser;
	private FirstPersonController FPC;
	private Camera FPcam;
	private Camera Lasercam;
	private TricorderHUD TriHUD;
	private ObjectivesUI ui;
	private BridgeController bc;
	private MasterControlScript mcs;
	private PauseMenu pauseMenu;

	public PickUpItem[] PickUpItem = new PickUpItem[4];


	public bool laserRemoved;
	public bool laserBuilt;
	public bool engineFixed;
	public GameObject Ring_red;
	public GameObject Ring_green;
	public Light EngineStatus;
	public Text[] EngContText = new Text[3];
	public AudioClip calmMusic;
	public O2Gauge o2_hud;



	int openLaser = Animator.StringToHash("OpenLaser");
	int triggerCam = Animator.StringToHash("TriggerCam");
	int closeLaser = Animator.StringToHash("CloseLaser");
	int reset = Animator.StringToHash("Reset");




	public bool tricorderCollected;


	void Start () {

		animLaser = this.gameObject.GetComponent<Animator> ();

		FPC = GameObject.Find ("FPSController").GetComponent<FirstPersonController>();
		FPcam = GameObject.Find ("FirstPersonCharacter").GetComponent<Camera> ();
		Lasercam = this.gameObject.GetComponentInChildren<Camera> ();
		TriHUD = GameObject.Find ("TricorderHUD").GetComponent<TricorderHUD> ();
		ui = GameObject.Find ("ObjectivesUI").GetComponent<ObjectivesUI> ();
		lppc = GameObject.Find ("LaserPartsHunt").GetComponent<LaserPartsPickupController> ();
		bc = GameObject.Find ("BridgeController").GetComponent<BridgeController> ();
		mcs = GameObject.Find ("DontDestroyonLoad").GetComponent<MasterControlScript> ();
		pauseMenu = GameObject.Find ("PauseMenu").GetComponent<PauseMenu> ();


		
	}
	
	void Update () {

		if (controllerActive == true)
		{
			Cursor.visible = true;	
			Cursor.lockState = CursorLockMode.None;
		}

		if (engineFixed == true) {
		}
			
	}

	void OnTriggerEnter(Collider other)
	{

		if (other.tag == "Player" && tricorderCollected == true  && laserRemoved == false && engineFixed == false) 
		{
			animLaser.SetBool (triggerCam, true);
			FPcam.enabled = false;
			FPC.enabled = false;
			Lasercam.enabled = true;
			controllerActive = true;
			StartCoroutine(TriHUD.ShowTricorderDiagnosisHUD ());
			ui.ActivateHUD (false);
			pauseMenu.pauseMenuAccessible = false;
		}

		if (other.tag == "Player" && tricorderCollected == true && laserRemoved == true && laserBuilt == true && engineFixed == false)
		{
			animLaser.SetBool (reset, false); 
			animLaser.SetBool (triggerCam, true);
			FPcam.enabled = false;
			FPC.enabled = false;
			Lasercam.enabled = true;
			controllerActive = true;
			StartCoroutine(TriHUD.ShowTricorderInstallHUD ());
			ui.ActivateHUD (false);
			pauseMenu.pauseMenuAccessible = false;


		}

	}

	public void OpenLaser()
	{

		animLaser.SetBool (openLaser, true);

	}

	public IEnumerator ReturntoFPC_withoutAdvance()
	{
		TriHUD.IAM.startDisappear ();
		yield return new WaitForSeconds (2f);
		controllerActive = false;
		FPcam.enabled = true;
		FPC.enabled = true;
		Lasercam.enabled = false;
		ui.ActivateHUD (true);
		animLaser.SetBool (closeLaser, true);
		yield return new WaitForSeconds (1.2f);
		ResetLaserAnimator ();
		pauseMenu.pauseMenuAccessible = true;




	}

	public IEnumerator ReturntoFPC()
	{
		TriHUD.IAM.startDisappear ();
		yield return new WaitForSeconds (2f);
		controllerActive = false;
		FPcam.enabled = true;
		FPC.enabled = true;
		Lasercam.enabled = false;
		ui.ActivateHUD (true);
		animLaser.SetBool (closeLaser, true);
		yield return new WaitForSeconds (1.2f);
		ResetLaserAnimator ();
		lppc.ActivateHUD ();
		ui.AdvanceObjective ();
		pauseMenu.pauseMenuAccessible = true;


//		PickUpItem [1].SetHaloOn(); 
//		PickUpItem [1].HuntActivated = true;
		for (int i = 0; i < PickUpItem.Length; i++)
		{
			PickUpItem [i].SetHaloOn ();
			PickUpItem [i].HuntActivated = true;
		}

	}

	public void ResetLaserAnimator()
	{
		animLaser.SetBool (openLaser, false);
		animLaser.SetBool (triggerCam, false);
		animLaser.SetBool (reset, true);
		animLaser.SetBool (closeLaser, false);

	}

	public IEnumerator EndSequence()
	{
		TriHUD.IAM.startDisappear ();
		yield return new WaitForSeconds (2f);
		controllerActive = false;
		FPcam.enabled = true;
		FPC.enabled = true;
		Lasercam.enabled = false;
		ui.ActivateHUD (true);
		animLaser.SetBool (closeLaser, true);
		yield return new WaitForSeconds (1.3f);
		TriHUD.ReflectProbe.gameObject.SetActive (false);

		ResetLaserAnimator ();
		Ring_red.SetActive (false);
		Ring_green.SetActive (true);
		EngineStatus.color = new Color32 (0, 255, 50, 255);
		this.engineFixed = true;
		ui.AdvanceObjective ();
		EngContText [0].text = "Online";
		EngContText [1].text = "Online";
		EngContText [2].text = "Engine Fully Operational";

		for (int i = 0; i < EngContText.Length; i++)
		{
			EngContText [i].color = Color.green;
		}

		bc.mainMusic.clip = calmMusic;
		bc.NormalLights ();
		o2_hud.RestoreO2 ();

		yield return new WaitForSeconds (3f);

		bc.intercom.clip = bc.intercomClip [5];
		bc.intercom.Play ();

		yield return new WaitForSeconds (10f);

		mcs.exitSequece = true;
		mcs.finalShipSequence = true;
		mcs.firstTimeInShip = false;
		mcs.backInDaShip = false;
		SceneManager.LoadScene ("IntroMarsOrbit");



	}


}
