using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class FPC_position : MonoBehaviour {



	private Vector3 Bridge_pos = new Vector3 (-23.4f, -2.7f, -26f);
	private Vector3 Bridge_rot = new Vector3 (0f, 14f, 0f);
	private Vector3 Holodeck_pos = new Vector3 (37f, -4.5f, 2.2f);
	private Vector3 Holodeck_rot = new Vector3 (0f, -98f, 0f);
	private Vector3 Crew_pos = new Vector3 (48.5f, -3f, -32.45f);
	private Vector3 Crew_rot = new Vector3 (0f, -70.6f, 0f);
	private Vector3 WakeUp_pos = new Vector3 (43.534f, -3.467f, -32.99f);
	private Vector3 WakeUp_rot = new Vector3 (0f, 0.01f, 0f);

	private GameObject ddol;




	MasterControlScript MCS;


	public FirstPersonController fpc;



	void Awake ()
	{
		

	}

	void Start () 
	{
		GameObject ddol = GameObject.Find("DontDestroyonLoad");
		MCS = ddol.gameObject.GetComponent<MasterControlScript>();



	}
	
	void Update ()
	{
		if (MCS.inBridge == true) {
			this.gameObject.transform.position = Bridge_pos;
//			this.gameObject.transform.rotation.eulerAngles = Bridge_rot;
			SetCameraToZero (Bridge_rot);
			MCS.inBridge = false;
		}

		if (MCS.inHolodeck == true) {
			this.gameObject.transform.position = Holodeck_pos;
//			this.gameObject.transform.rotation.eulerAngles =  Holodeck_rot;
			SetCameraToZero (Holodeck_rot);
			MCS.inHolodeck = false;
		}

		if (MCS.inCrew == true) {
			this.gameObject.transform.position = Crew_pos;
//			this.gameObject.transform.rotation.eulerAngles = Crew_rot;
			SetCameraToZero (Crew_rot);
			MCS.inCrew = false;
		}

		if (MCS.wokeUp == true) {
			this.gameObject.transform.position = WakeUp_pos;
//			this.gameObject.transform.rotation.eulerAngles = WakeUp_rot;
			SetCameraToZero (WakeUp_rot);
			MCS.wokeUp = false;

		}

	}
	public void SetCameraToZero(Vector3 vect)  //set camera rotation to zero using FPC scripts - http://answers.unity3d.com/questions/835931/rotate-first-person-controller-via-script.html
		{
			fpc.m_MouseLook.m_CameraTargetRot = Quaternion.Euler (Vector3.zero);
			fpc.m_MouseLook.m_CharacterTargetRot = Quaternion.Euler (vect);

		}


}
