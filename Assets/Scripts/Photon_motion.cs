using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photon_motion : MonoBehaviour {


	public Vector3 Velocity = new Vector3(0,0,0);
	public float rotSpeed = 10f;
	private float x;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		this.transform.position += Velocity * Time.deltaTime;

		x += Time.deltaTime * rotSpeed;
		this.transform.localRotation = Quaternion.Euler (x, 0, 0);
//		this.transform.RotateAround(this.transform.position,Vector3.left,rotSpeed*Time.deltaTime);
//		this.transform.Rotate(Vector3.up,rotSpeed * Time.deltaTime);
//		transform.rotation = Quaternion.AngleAxis(rotSpeed * Time.deltaTime,Vector3.up);
	}
}
