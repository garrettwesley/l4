using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class Killvolume : MonoBehaviour {

	public FirstPersonController fpc;
	public Transform[] RespawnPoints;
	public int spawnNum;


	void Start () {
		
	}
	
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			this.RespawnPlayer ();


		}

	}

	public void RespawnPlayer ()
	{
		fpc.gameObject.transform.position = RespawnPoints[spawnNum].position;
//		fpc.gameObject.transform.rotation.SetLookRotation(RespawnPoints[spawnNum].rotation.eulerAngles);
		fpc.gameObject.transform.eulerAngles = RespawnPoints[spawnNum].rotation.eulerAngles;
//		fpc.gameObject.transform.eulerAngles = new Vector3 (0,88.5f,0);
		print ("fpc" + fpc.gameObject.transform.eulerAngles);
		print (RespawnPoints [6].rotation.eulerAngles);
		SetCameraToZero ();




	}



	public void SetCameraToZero()  //set camera rotation to zero using FPC scripts - http://answers.unity3d.com/questions/835931/rotate-first-person-controller-via-script.html
	{
		fpc.m_MouseLook.m_CameraTargetRot = Quaternion.Euler (Vector3.zero);
		fpc.m_MouseLook.m_CharacterTargetRot = Quaternion.Euler (RespawnPoints[spawnNum].rotation.eulerAngles);

	}


}
