using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;


public class reloadSpaceship: MonoBehaviour {



	private bool laserLevel;


	public bool HolodeckComplete;

	private MasterControlScript MCS;

	// Use this for initialization
	void Start () {
		GameObject ddol = GameObject.Find("DontDestroyonLoad");
		MCS = ddol.gameObject.GetComponent<MasterControlScript>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		
		SceneManager.LoadScene("spaceship_master2");
		MCS.backInDaShip = true;
		MCS.inHolodeck = true;

	}

	public void ExitNexis ()
	{
		SceneManager.LoadScene("spaceship_master2");
		MCS.inHolodeck = true;


	}
}
