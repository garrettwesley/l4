using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;

public class SleepQuarters : MonoBehaviour {


	/* Script controls triggers sleep quarters camera to turn on.  The end of the camera animation (Camera_sleepquarter) triggers the animator on SleepQuarter/SleepyEyes_UI/Canvas
	(SleepyEyes) to turn on.  Unfortuntaly, can't set up an event trigger for the camera inside of the Canvas animation because it sits deeper in the hierachy than camera.
	So I was forced to make another script attached to Canvas that triggers a function in this script.  */

	private GameObject ddol;
	private bool DoOnce;

	public FirstPersonController FPC;
	public Animator animCam;
	public Animator animSleep;
	public Camera FPCam;
	public Camera sleepCam;
	public AudioListener audioListen;
	public AudioSource BridgeAudioPlayer;
	public AudioClip redalert;
	public GameObject sleepCanvas;
	public GameObject halo;
	public AudioSource sleepScript;
	public O2Gauge o2_screen;
	public O2Gauge o2_hud;


	ObjectivesUI objUI;
	MasterControlScript MCS;
	AudioListener FPCamListener;
	AudioListener sleepCamListner; 
	BridgeController bc;




	int sleepCameraOn = Animator.StringToHash("TurnSleepOn");
	int sleepyEyesOn = Animator.StringToHash("SleepEyes");

	// ------------------------------------------------------------------------------------- //


	void Start () 
	{
		

		GameObject ddol = GameObject.Find("DontDestroyonLoad");
		MCS = ddol.gameObject.GetComponent<MasterControlScript>();

		bc = GameObject.Find ("BridgeController").GetComponent<BridgeController> ();


		GameObject objective = GameObject.Find ("ObjectivesUI");
		objUI = objective.gameObject.GetComponent<ObjectivesUI> ();

		FPCamListener = FPCam.GetComponentInParent<AudioListener> ();
		sleepCamListner = sleepCam.GetComponentInParent<AudioListener> ();
	}

	// ------------------------------------------------------------------------------------- //

	
	void Update () {



//		if(MCS.backInDaShip == true)
//		{
//			sleepCanvas.SetActive (true);
//			halo.SetActive (true);
//		}
		
	}
	// ------------------------------------------------------------------------------------- //

	void OnTriggerEnter(Collider other)
	{

		if (other.tag == "Player"  && MCS.backInDaShip == true && DoOnce == false  ) 
		{
			sleepScript.Play ();
			FPC.enabled = false;
			FPCam.enabled = false;
			sleepCam.enabled = true;
			sleepCanvas.SetActive (true);
			animCam.SetBool (sleepCameraOn, true);
			StartCoroutine(bc.VolumeFade_down (0f));


			halo.SetActive (false);

			FPCamListener.enabled = false;
			sleepCamListner.enabled = true;
			objUI.ActivateHUD (false);

			DoOnce = true;


		}
	}

	public IEnumerator sleepSequece()
	{
		yield return new WaitForSeconds (20f);
		animCam.SetBool (sleepCameraOn, true);
	}


	// ------------------------------------------------------------------------------------- //


	public void TriggerSleepyEyes()
	{
		animSleep.SetBool (sleepyEyesOn, true);

	}

	// ------------------------------------------------------------------------------------- //


	public void TriggerCameraEndSequence()
	{
		animSleep.SetBool (sleepyEyesOn, false);
		StartCoroutine (WakeUpDelay ());
		BridgeAudioPlayer.clip = redalert;
		BridgeAudioPlayer.Play ();
		StartCoroutine(bc.VolumeFade_up (0.6f));

	}

	public IEnumerator WakeUpDelay()
	{
		yield return new WaitForSeconds (2f);
		animCam.SetBool (sleepCameraOn, false);

	}

	public void ReturntoFPC()
	{
		objUI.ActivateHUD (true);
		FPC.enabled = true;
		FPCam.enabled = true;
		sleepCam.enabled = false;
		FPCamListener.enabled = true;
		sleepCamListner.enabled = false;
		MCS.wokeUp = true;
		objUI.AdvanceObjective ();
		bc.RedAlertScreen ();
		sleepCanvas.SetActive (false);
		halo.SetActive (false);
		o2_hud.gameObject.SetActive (true);
		o2_hud.StartO2Counter ();
		o2_screen.StartO2Counter ();
	}
}
