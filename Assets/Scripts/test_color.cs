using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine;

public class test_color : MonoBehaviour {


	public Image image_ring;
	public Material image2;

	// Use this for initialization
	void Start () {

		//image_ring = GetComponent<Image> ();

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.K))
		{

			image_ring.material = image2;



		}


	}
}
