﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPC_tracker : MonoBehaviour {



	void Awake()
	{
		DontDestroyOnLoad (this.transform.gameObject);

		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}


	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
