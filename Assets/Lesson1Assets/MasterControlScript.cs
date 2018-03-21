using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterControlScript : MonoBehaviour {


	public bool holoDeckLoaded;
	public bool bridgeDoorUnlocked;
	public bool crewDoorsUnlocked;
	public bool labDoorsUnlocked;
	public bool engineDoorUnlocked;
	public bool badgePickedUp;

	public bool inBridge;
	public bool inCrew;
	public bool inHolodeck;
	public bool wokeUp;

	public bool firstTimeInShip;
	public bool backInDaShip;
	public bool reloadRedAlert;


	public bool exitSequece;
	public bool finalShipSequence;
	public bool gameCompleted;







//	Door_Master[] door = new Door_Master[5];   //creating a variable for the script Door_Master
//	Badge_script badge_s;
//	public GameObject bridgeDoor;//creating a varaible for the door that I want to open
//	public GameObject crewDoor1;
//	public GameObject crewDoor2;
//	public GameObject labDoor1;
//	public GameObject labDoor2;
//	public GameObject badge;


	void Start () {



//		GameObject bridgeDoor = GameObject.Find("Door_Bridge"); // set variable to actural game object without needing to drag from editor.
//		GameObject crewDoor1 = GameObject.Find("Door_Crew1");
//		GameObject crewDoor2 = GameObject.Find("Door_Crew2");
//		GameObject labDoor1 = GameObject.Find("Door_Lab1");
//		GameObject labDoor2 = GameObject.Find("Door_Lab2");
//		GameObject badge = GameObject.Find ("Badge");
//		door[0] = bridgeDoor.GetComponent<Door_Master> ();  //assign door to component Door_Master on labDoor1
//		door[1] = crewDoor1.GetComponent<Door_Master> ();
//		door[2] = crewDoor2.GetComponent<Door_Master> ();
//		door[3]= labDoor1.GetComponent<Door_Master> ();
//		door[4]= labDoor2.GetComponent<Door_Master> ();
//		badge_s = badge.GetComponent<Badge_script> ();


	}
	void Awake()
	{
		DontDestroyOnLoad (this.transform.gameObject);

		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}
	

	}


	
	// Update is called once per frame
	void Update () {

//		if (bridgeDoorUnlocked == true)
//		{
//			door[0].unlockDoor();
//		}
//
//		if (crewDoorsUnlocked == true)
//		{
//			door[1].unlockDoor();
//			door[2].unlockDoor();
//		}
//
//		if (labDoorsUnlocked == true)
//		{
//			door[3].unlockDoor();
//			door[4].unlockDoor();
//		}
//
//		if (badgePickedUp == true && holoDeckLoaded == true)
//		{
//			badge_s.destroyBadge ();
//		}

	}
}
