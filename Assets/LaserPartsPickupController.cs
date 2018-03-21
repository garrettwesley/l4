using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LaserPartsPickupController : MonoBehaviour {



	private Color grey = new Color32(50,50,50,150);
	private bool doneOnce;
	private ObjectivesUI ui;


	public Image Image_Laserbody;
	public Image Image_MirrorFull;
	public Image Image_MirrorPartial;
	public Image Image_Battery;

	public GameObject LaserCheck;
	public GameObject PartialMirrorCheck;
	public GameObject FullMirrorCheck;
	public GameObject BatteryCheck;

	public bool LaserPickUp;
	public bool PartialMirrorPickUp;
	public bool FullMirrorPickUp;
	public bool BatteryPickUp;
	public GameObject LaserPartsHUD;
	public GameObject FullLaserHUD;


	public bool LaserPartsFound;





	// ------------------------------------------------------------------------------------- //


	void Start () {

		ui = GameObject.Find ("ObjectivesUI").GetComponent<ObjectivesUI> ();



	}
	
	void Update () {

		if(doneOnce == false)
		{
			if (LaserPickUp == true && PartialMirrorPickUp == true && FullMirrorPickUp == true && BatteryPickUp == true) 
			{
				LaserPartsFound = true;
				ui.AdvanceObjective ();
				doneOnce = true;
			}
		}



	}

	// ------------------------------------------------------------------------------------- //




	public void ActivateHUD()
	{
		this.LaserPartsHUD.SetActive (true);
	}

	// ------------------------------------------------------------------------------------- //


	public void LaserFound()
	{
		Image_Laserbody.color = grey;
		LaserCheck.SetActive (true);	
	}

	// ------------------------------------------------------------------------------------- //

	public void PartialMirrorFound()
	{
		Image_MirrorPartial.color = grey;
		PartialMirrorCheck.SetActive (true);
	}

	// ------------------------------------------------------------------------------------- //

	public void FullMirrorFound()
	{
		Image_MirrorFull.color = grey;
		FullMirrorCheck.SetActive (true);
	}

	// ------------------------------------------------------------------------------------- //

	public void BatteryFound()
	{
		Image_Battery.color = grey;
		BatteryCheck.SetActive (true);
	}

	// ------------------------------------------------------------------------------------- //

	public void FullLaserFound()
	{
		FullLaserHUD.SetActive (true);

	}

	// ------------------------------------------------------------------------------------- //

	public void ResetHUD()
	{
		Image_Laserbody.color = Image_MirrorFull.color = Image_MirrorPartial.color = Image_Battery.color =  Color.white;
		LaserCheck.SetActive (false);
		PartialMirrorCheck.SetActive (false);
		FullMirrorCheck.SetActive (false);
		BatteryCheck.SetActive (false);

	}

	public void RemoveCompletedLaserHUD()
	{
		FullLaserHUD.SetActive (false);


	}


}

