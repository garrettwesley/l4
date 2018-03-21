using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMG_reciever : MonoBehaviour {


	public LaserMiniGame lmg;
	public GameObject singlePhoton;
	public GameObject doublePhoton;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == singlePhoton)
		{
//			lmg.AdvanceToDoubleExcitation ();

		}

	}

}
