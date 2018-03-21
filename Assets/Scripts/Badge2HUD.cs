using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Badge2HUD : MonoBehaviour {


	public GameObject HUDobject;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		}


	public IEnumerator badgeHUD ()
	{
		Debug.Log ("badgecalled");
		HUDobject.gameObject.active = true;
		yield return new WaitForSeconds (3f);
		HUDobject.gameObject.active = false;
	}


}


