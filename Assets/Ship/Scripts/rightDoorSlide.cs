using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rightDoorSlide : MonoBehaviour {

	public int DoorSpeed = 5;

	public bool doorActive = true;

	doorSlide door;



	void Start () {

		door = GetComponentInParent<doorSlide> ();



	}

	void Update () {

		if (door.triggerOverlap == true && transform.localPosition.z > -3.75 && doorActive == true) {
			Open ();
		}

		if (door.triggerOverlap == false && transform.localPosition.z < -0.15 && doorActive == true) {
			Close ();
		}
	}

	public void ActivateDoor (bool isActive)
	{
		doorActive = isActive;


	}


	void Open()
	{
		transform.Translate (0, 0, -DoorSpeed * Time.deltaTime,  Space.World);

	}

	void Close()
	{
		transform.Translate (0, 0, DoorSpeed * Time.deltaTime, Space.World);

	}
}

