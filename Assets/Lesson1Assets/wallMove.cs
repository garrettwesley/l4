using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallMove : MonoBehaviour {

	public bool openWall;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(openWall == true && this.gameObject.transform.position.y> -3)
		{

			this.gameObject.transform.Translate (Vector3.down,Space.World);

		}
		
	}
}
