using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepCamTrigger : MonoBehaviour {


	public SleepQuarters sleepquarters;
	public BridgeController bc;


	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangetoRedAlertColor()
	{
		bc.RedAlertSequence ();

	}

	public void BridgeControllerTrigger()
	{
		bc.RedAlertActive = true;
		StartCoroutine (bc.LerpColorUp ());

	}


	public void CameraTrigger()
	{
		sleepquarters.TriggerCameraEndSequence ();

	}

	public void SheepSound()
	{
		this.GetComponent<AudioSource> ().Play ();
	}
}
