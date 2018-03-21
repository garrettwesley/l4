using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Badge_script : MonoBehaviour {

	public GameObject HUDobject;
	public GameObject Badge2Destroy;
	public ObjectivesUI ui;
	public GameObject halo;
	public Door_Master NexisDoor;



	private GameObject labDoor1;
	private GameObject labDoor2;
	private GameObject ddol;

	MasterControlScript MCS;
	Door_Master door1; 
	Door_Master door2;
	BridgeController bc;



	// Use this for initialization
	void Start () {

		GameObject labDoor1 = GameObject.Find("Door_Lab1");
		GameObject labDoor2 = GameObject.Find("Door_Lab2");
		door1= labDoor1.GetComponent<Door_Master> ();
		door2= labDoor2.GetComponent<Door_Master> ();
		GameObject ddol = GameObject.Find("DontDestroyonLoad");
		MCS = ddol.gameObject.GetComponent<MasterControlScript>();
		bc = GameObject.Find ("BridgeController").GetComponent<BridgeController> ();

		
	}
	
	// Update is called once per frame
	void Update () {

		if(MCS.badgePickedUp == true && MCS.holoDeckLoaded == true)
		{
			destroyBadge ();
		}


	

	}


	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") 
		{
			door1.unlockDoor ();
			door2.unlockDoor ();
			MCS.labDoorsUnlocked = true;
			MCS.badgePickedUp = true;
			ui.AdvanceObjective ();

			StartCoroutine (badgeHUD());  //coroutine ends in destorying game object - must be called last


		}
	
	}

	public IEnumerator badgeHUD ()
	{
		this.gameObject.GetComponent<Renderer> ().enabled = false;
		this.halo.SetActive (false);

		HUDobject.gameObject.SetActive (true);

		yield return new WaitForSeconds (5f);
		HUDobject.gameObject.SetActive(false);
		yield return new WaitForSeconds (2f);

		bc.intercom.clip = bc.intercomClip [2];
		bc.intercom.Play ();
		NexisDoor.unlockDoor ();
		Destroy (this.gameObject);
	}


	public void destroyBadge()
	{
		Destroy (Badge2Destroy);
	}

}
//	public void Appear()
//	{
//		HUDobject.gameObject.active = true;
//
//	}
//
//	public void Disappear()
//	{
//		HUDobject.gameObject.active = false;
//
//
//	}

