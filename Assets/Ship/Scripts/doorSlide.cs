using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorSlide : MonoBehaviour {

	public int DoorSpeed = 5;
	public bool triggerOverlap =  false;
	private int doorPosition = 1; // 1 for door closed, 2 for door open




	void Start () {



	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			triggerOverlap = true;

		}
	}



	void OnTriggerExit(Collider other)
	{	
		if (other.tag == "Player") 
		{
			triggerOverlap = false;
		}
	}


	void Update () 
	{


	}




	//

}