using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class translate : MonoBehaviour 
{


	void Start () 
	{
		
	}
	

	void Update () 
	{
		float translate = Time.deltaTime;
		do {
			
			transform.Translate (0, 0, translate);
		} while(transform.position.z < 23);


	}
}
	