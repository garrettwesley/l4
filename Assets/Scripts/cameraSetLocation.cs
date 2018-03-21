using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;



public class cameraSetLocation : MonoBehaviour {
	


	public Vector3 camNextLocation = new Vector3(32f,-4.5f, -2.1f);
	public Vector3 camNextRotation = new Vector3 (0f, 280f, 0f);
	MasterControlScript MCS;
	private GameObject ddol;
	public FirstPersonController FPC; 


//	-14,0.5, -6
//	0,75,0

	void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}

	}


	void Start () {

		GameObject ddol = GameObject.Find("DontDestroyonLoad");
		MCS = ddol.gameObject.GetComponent<MasterControlScript>();

//		FPC = this.gameObject.GetComponentInChildren<FirstPersonController> ();
//		camera = this.gameObject.GetComponentInChildren<FirstPersonCharacter>
//		this.FPC.GetComponent<FirstPersonController> ().enabled = true;
//		camera.enabled = true;


		
	}
	
	// Update is called once per frame
	void Update () {

//		if (MCS.holoDeckLoaded == true)
//		{
//			OnLevelWasLoaded ();
//		}
//	}
//
//	public void OnLevelWasLoaded ()
//
//	{
//
//		this.transform.position = camNextLocation;
//		this.transform.eulerAngles = camNextRotation;
//		this.FPC.GetComponent<FirstPersonController> ().enabled = true;
//		camera.enabled = true;
	}


}
