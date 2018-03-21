using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playAudioOnKey : MonoBehaviour {


	public AudioSource audClip;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Space))
		{
			audClip.Play();
		} 
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			audClip.Pause ();
		}

		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			audClip.UnPause ();
		}

	}
}
