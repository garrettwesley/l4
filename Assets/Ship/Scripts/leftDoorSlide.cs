using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leftDoorSlide : MonoBehaviour {


	public int DoorSpeed = 5;

	public bool doorActive = true;

	rightDoorSlide rightdoor;
	doorSlide door;

	void Start () {

		door = GetComponentInParent<doorSlide> ();
		rightdoor = GameObject.Find ("rightDoor").GetComponent<rightDoorSlide> ();


	}
	
	void Update () {

		if (door.triggerOverlap == true && transform.localPosition.z < 12.3 && doorActive == true ) {
			Open ();

		}

		if (door.triggerOverlap == false && this.transform.localPosition.z > 8.6 && doorActive == true) {
			Close ();
		}
	}

	public void ActivateDoor (bool isActive)
	{
		doorActive = isActive;
		rightdoor.ActivateDoor (isActive);

	}


	void Open()
		{
		transform.Translate (0, 0, DoorSpeed * Time.deltaTime,  Space.World);

		}

	void Close()
		{
		transform.Translate (0, 0, -DoorSpeed * Time.deltaTime, Space.World);

		}
	}

