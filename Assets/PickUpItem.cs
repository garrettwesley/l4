using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour {

	private LaserPartsPickupController lppc;
	private EngineLaserController elc;

	public Animator haloanim;
	public bool Laserbody;
	public bool Fullmirror;
	public bool Partialmirror;
	public bool Battery;
	public bool FullLaser;
	public bool HuntActivated;

	int haloOn = Animator.StringToHash("haloOn");

	// ------------------------------------------------------------------------------------- //


	void Start () {

		lppc = GameObject.Find ("LaserPartsHunt").GetComponent<LaserPartsPickupController> ();
		elc = GameObject.Find ("EngineLaser").GetComponent<EngineLaserController> ();

	}

	// ------------------------------------------------------------------------------------- //


	void Update () {


	}

	// ------------------------------------------------------------------------------------- //

	public void SetHaloOn()
	{
		haloanim.SetBool(haloOn, true);

	}

	// ------------------------------------------------------------------------------------- //

	public void SetHaloOff()
	{
		haloanim.SetBool (haloOn, false);

	}

	// ------------------------------------------------------------------------------------- //


	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && HuntActivated == true) 
		{
			if (Laserbody == true) 
			{
				this.gameObject.SetActive (false);
				lppc.LaserFound ();
				lppc.LaserPickUp = true;

			}

			if (Fullmirror == true) 
			{
				this.gameObject.SetActive (false);
				lppc.FullMirrorFound ();
				lppc.FullMirrorPickUp = true;

			}


			if (Partialmirror == true) 
			{
				this.gameObject.SetActive (false);
				lppc.PartialMirrorFound ();
				lppc.PartialMirrorPickUp = true;

			}

			if (Battery == true) 
			{
				this.gameObject.SetActive (false);
				lppc.BatteryFound ();
				lppc.BatteryPickUp = true;
			}

			if (FullLaser == true)
			{
				this.gameObject.SetActive (false);
				elc.laserBuilt = true;
				lppc.FullLaserFound ();
			}
		}
		
		
	}

		


}