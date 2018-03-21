using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointSetter : MonoBehaviour {


	private Killvolume kv;

	public int checkpointNum;


	void Start () {

		kv = GameObject.Find ("KillVolume").GetComponent<Killvolume> ();
		
	}
	
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			kv.spawnNum = checkpointNum;
		}
	}

}
