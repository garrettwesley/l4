using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsIntroRotation : MonoBehaviour {


	public bool Mars;
	public bool Astronaut;
	public bool Station;
	public float speed;
	public Vector3 velocity;
	public Vector3 rotationDirection;
	public Vector3 stationRot;




	void Start () {
		
	}


	
	void Update () 
	{


		if (Mars == true)
		{
			gameObject.transform.Rotate (Vector3.up, speed * Time.deltaTime);
		}

		if (Astronaut == true)
		{
			gameObject.transform.Translate (velocity * Time.deltaTime, Space.World);
			gameObject.transform.Rotate (rotationDirection, speed * Time.deltaTime);

		}

		if (Station == true)
		{
			gameObject.transform.Rotate (stationRot, speed * Time.deltaTime);

		}
		
	}





		


}
