using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_test : MonoBehaviour {

	Door_Master door;   //creating a variable for the script Door_Master
	private GameObject labDoor1;  //creating a varaible for the door that I want to open


	void Start ()
	{
		GameObject labDoor1 = GameObject.Find("Door_Lab1"); // set variable to actural game object without needing to drag from editor.

		door = labDoor1.GetComponent<Door_Master> ();  //assign door to component Door_Master on labDoor1


	}

	void OnTriggerEnter(Collider other)
	{


		if(other.tag == "Player")
		{
			Debug.Log ("yo");
			door.unlockDoor ();  //call function unlockDoor and pass parameter 0 to unlockDoor() on door


		}

	}


}
