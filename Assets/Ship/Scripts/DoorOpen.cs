using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour {

	// Use this for initialization

//	public static DoorOpen Instance;
//
//	void Awake()
//	{
//		Instance = this;
//	}
//	

	public AudioSource doorsound;
	public int DoorSpeed = 5;
	public bool triggerOverlap =  false;
	private int doorPosition = 1; // 1 for door closed, 2 for door open
//	public float doorFrame = 0.1f;
//	public float doorTime = 20f;


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
		if(triggerOverlap == true && transform.position.y < 5)
			{
				OpenDoor();
			}

		if (triggerOverlap == false && transform.position.y > -3) 
			{
				CloseDoor ();
			}
			

	}

	void OpenDoor ()
		{
			transform.Translate (0,DoorSpeed*Time.deltaTime,0);
			doorsound.Play ();
		}

	void CloseDoor ()
		{
			transform.Translate (0,-DoorSpeed*Time.deltaTime,0);
			doorsound.pitch = -1f;
			doorsound.Play ();
		}



			//

}
//	}

//	public void overlapTrigger()
//	{
//		StartCoroutine (DoorTrigger (TimeSpan.FromSeconds (this.doorFrame)));
//	}


//	private IEnumerator DoorTrigger (TimeSpan  doorTime)
//	{	
//		float doorPos = 1000f;
//		if (triggerOverlap == true)
//		{
//			Debug.Log ("trigger on");
//			Debug.Log (transform.position.y);
//			while (transform.position.y < 5)
//			{
//				transform.Translate(0, doorPos, 0);
//				yield return new WaitForSeconds((float)doorTime.TotalSeconds);
//				Debug.Log ("Success");
//			}
//		}
//
//
//	}
//




