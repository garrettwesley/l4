using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class TricorderHUD : MonoBehaviour {

	private LaserPartsPickupController lppc;
	private bool doOnce;


	public Button[] button = new Button[5];
	public GameObject DiagnoseImage;
	public GameObject BrokenLaser;
	public GameObject FixedLaser;
	public GameObject ReflectProbe;
	public GameObject LaserBeam;




	public InterfaceAnimManager IAM;
	EngineLaserController ELC;


	// ------------------------------------------------------------------------------------- //

	void Start () 
	{

		IAM = this.gameObject.GetComponent<InterfaceAnimManager> ();
		ELC = GameObject.Find ("EngineLaser").GetComponent<EngineLaserController> ();
		lppc = GameObject.Find ("LaserPartsHunt").GetComponent<LaserPartsPickupController> ();


	}

	// ------------------------------------------------------------------------------------- //

	void Update () 

	{

	}

	// ------------------------------------------------------------------------------------- //


	public IEnumerator ShowTricorderDiagnosisHUD()
	{
		yield return new WaitForSeconds(3f);
		this.IAM.startAppear ();
		yield break;
	}

	// ------------------------------------------------------------------------------------- //


	public IEnumerator ShowTricorderInstallHUD()
	{
		yield return new WaitForSeconds(3f);
		this.IAM.startAppear ();
		button [3].gameObject.SetActive (true);
		yield break;
	}

	// ------------------------------------------------------------------------------------- //

	public void Diagnose()
	{
		if(doOnce == false)
		{
			this.DiagnoseImage.SetActive (true);
			StartCoroutine (ButtonAppear ());
			doOnce = true;
		}

		
	}

	// ------------------------------------------------------------------------------------- //


	public IEnumerator ButtonAppear()
	{
		yield return new WaitForSeconds (3f);
		button [2].gameObject.SetActive (true);
		yield break;

	}

	// ------------------------------------------------------------------------------------- //


	public void Exit()
	{
		this.DiagnoseImage.SetActive (false);
		StartCoroutine(ELC.ReturntoFPC_withoutAdvance ());
	}

	// ------------------------------------------------------------------------------------- //


	public void RemoveLaser()
	{
		StartCoroutine (RemoveLaser1());  //Can't call a pulbic IEnumerator from an Animation event trigger, so this functionc calls the actual function below.


	}

	// ------------------------------------------------------------------------------------- //

	public IEnumerator RemoveLaser1()
	{
		BrokenLaser.SetActive (false);
		button [2].gameObject.SetActive (false);
		this.DiagnoseImage.SetActive (false);
		yield return new WaitForSeconds (2f);
		ELC.laserRemoved = true;
		StartCoroutine(ELC.ReturntoFPC ());
		yield break;

	}

	// ------------------------------------------------------------------------------------- //

	public void PlaceFixedLaser ()
	{
		StartCoroutine (PlaceFixedLaser1 ());

	}

	// ------------------------------------------------------------------------------------- //

	public IEnumerator PlaceFixedLaser1()
	{
		FixedLaser.gameObject.SetActive (true);
		lppc.RemoveCompletedLaserHUD ();
		button [3].gameObject.SetActive (false);
		yield return new WaitForSeconds (1f);
		ELC.engineFixed = true;
		button [4].gameObject.SetActive (true);
		yield break;
	}

	// ------------------------------------------------------------------------------------- //

	public void LaserActivate()
	{
		StartCoroutine (LaserActivated1 ());

	}

	// ------------------------------------------------------------------------------------- //

	public IEnumerator LaserActivated1()
	{
		LaserBeam.gameObject.SetActive (true);
		ReflectProbe.gameObject.SetActive (true);
		button [4].gameObject.SetActive (false);
		StartCoroutine(ELC.EndSequence ());
		yield break;
	}

}
