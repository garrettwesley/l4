using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Spontaneous_emission : MonoBehaviour {

	private GameObject photon1;
	private GameObject photon2;
	private int photonCounter = 0;
	private bool doOnce;
	private Vector3[] transPos = new Vector3[3];




	public GameObject photon;
	public GameObject photonDouble;
	public GameObject photon_noRB;
    public Transform StartPosition;
    public Transform DoubleStartPosition;
	public Vector3 Velocity;
	public bool photonCreated;
	public bool doublePhotonCreated;
	public bool HeNe_excitation;
	public bool sponEmission;
    public bool Tinyphoton;
	public bool atomPhoton;
	public bool loop;

	public bool laserCavityDemo;

	public Vector3 rbVelocity;
	public bool Xemission;
	public bool Yemission;
	public bool Zemission;
	public Vector3 spawnerDisplacement;
	public Transform[] trans;
	public Text NumPhotonText;
	public GameObject EmptyGO;
	public HeliumAtom HeAtom;








	void Start () {
		if(laserCavityDemo == true)
		{
			for (int i = 0; i < trans.Length; i++)
			{
				transPos [i] = trans [i].position;
			}
					
		}
	   }
	
	//-----------------------------------------------------------------------------------//

	void Update ()
	{  
//		if(laserCavityDemo == true)
//		{
//			if (Input.GetKeyDown (KeyCode.L)) {
//
//				bouncyPhoton(1);
//			}
//		}
//
//
//		if (Input.GetKeyDown (KeyCode.P)) {
//		
//			SpawnPhoton_sponEm ();
//		}
			

		if (photon1 != null && photonCreated == true)
		{
			this.photon1.transform.position -= this.Velocity * Time.deltaTime;
		}	

		if (photon2 != null  && doublePhotonCreated == true)
		{
          //  photon2.transform.position = Position.position - new Vector3(5, 0, 0);
            photon2.transform.position -= this.Velocity * Time.deltaTime;
		}	
			
	}

	//-----------------------------------------------------------------------------------//


	void OnTriggerEnter(Collider other)
	{
		if (atomPhoton == true) {
			Destroy (photon1);

		}
	}

	//-----------------------------------------------------------------------------------//

	void OnTriggerExit(Collider other)
	{

		if (laserCavityDemo == true) {

			if (Xemission == true) {
				bouncyPhoton (Mathf.Sign (other.gameObject.GetComponent<Rigidbody> ().velocity.x));
			}

			if (Yemission == true) {
				bouncyPhoton (Mathf.Sign (other.gameObject.GetComponent<Rigidbody> ().velocity.y));
			}

			if (Zemission == true) {
				bouncyPhoton (Mathf.Sign (other.gameObject.GetComponent<Rigidbody> ().velocity.z));
			}
		}

	}


	//-----------------------------------------------------------------------------------//

	public void CreateBouncyPhoton()
	{
		bouncyPhoton(1);
	}

	//-----------------------------------------------------------------------------------//

	public void SpawnPhoton_sponEm()
	{


			this.photon1 = InstantiatePhoton ();
        //	this.photon1.transform.position = this.gameObject.transform.position;
            this.photon1.transform.position = StartPosition.transform.position;


            this.photonCreated = true;

	}


	public void SpawnPhoton_double()
	{


		this.photon2 = InstantiatePhoton_double ();

//		if (sponEmission == true) 
//		{
//            //	this.photon2.transform.position = this.gameObject.transform.position + new Vector3(0,0,0);
//            this.photon2.transform.position = this.gameObject.transform.position;
//
//        }
		if (HeNe_excitation == true) 
		{
			this.photon2.transform.position = this.gameObject.transform.position;
		}

		this.doublePhotonCreated = true;

	}

	//-----------------------------------------------------------------------------------//


	public void DestroyPhoton()
	{
		
		Destroy (this.photon1);
		Destroy (this.photon2);

	}

	//-----------------------------------------------------------------------------------//

	public void bouncyPhoton(float vDirection)
	{
		if (photonCounter < 512) {
			doOnce = false;

			if (vDirection < 0) {
				trans [0].transform.Translate (spawnerDisplacement, Space.World);
				this.photon1 = InstantiatePhoton_laserCavity (trans [0]);
				photon1.GetComponentInChildren<Rigidbody> ().velocity = vDirection * rbVelocity;
				photonCounter++;
				NumPhotonText.text = photonCounter.ToString ();

				if (photonCounter % 4 == 0)
				{
					trans [2].transform.Translate (spawnerDisplacement, Space.World);
					this.photon2 = InstantiatePhoton_laserCavity_noRB (trans [2]);
					photon2.GetComponentInChildren<Rigidbody> ().velocity = vDirection * rbVelocity;
				}
			}
			if (vDirection > 0) {
				trans [1].transform.Translate (-spawnerDisplacement, Space.World);
				this.photon1 = InstantiatePhoton_laserCavity (trans [1]);
				photon1.GetComponentInChildren<Rigidbody> ().velocity = vDirection * rbVelocity;
				photonCounter++;
				NumPhotonText.text = photonCounter.ToString ();
			}

		}

		if(photonCounter == 512 && doOnce == false)
		{
			doOnce = true;
			StartCoroutine (DestroyLaserPhotons ());

		}
			
	}

	//-----------------------------------------------------------------------------------//


	public IEnumerator DestroyLaserPhotons()
	{
		yield return new WaitForSeconds (3f);
		var clones = GameObject.FindGameObjectsWithTag ("Photon");
		foreach (var clone in clones)
		{
			Destroy(clone);
		}
		doOnce = false;
		photonCounter = 0;

		for (int i = 0; i < trans.Length; i++) {
			trans [i].position = transPos [i];
		}
			bouncyPhoton (1);
	}

	//-----------------------------------------------------------------------------------//

	public IEnumerator RestartStimEm()
	{
		yield return new WaitForSeconds (2f);
		HeAtom.Excite ();
	}

	//-----------------------------------------------------------------------------------//

	private GameObject InstantiatePhoton()
	{
		GameObject result = GameObject.Instantiate (this.photon, StartPosition.position, StartPosition.rotation);
		if (Tinyphoton == true)
		{
			result.transform.localScale = new Vector3(.1f, .1f, .1f);
		}
		//  photon.SetActive(true);
		// Destroy(this.photon1, 2);
		return result;
	}

	//-----------------------------------------------------------------------------------//

	private GameObject InstantiatePhoton_double()
	{

		//   Vector3 spawnPosition = transform.position + localOffset;
		//     Transform localOffset = StartPosition;
		//     localOffset.transform.position = StartPosition.position - new Vector3(2, -0.5, 0);
		GameObject result = GameObject.Instantiate (this.photonDouble, DoubleStartPosition.position, DoubleStartPosition.rotation);
		//  photonDouble.SetActive(true);
		if(loop == true)
		{
			StartCoroutine (RestartStimEm ());
		}

		Destroy(result, 4f);
		return result;
	}

	//-----------------------------------------------------------------------------------//

	private GameObject InstantiatePhoton_laserCavity(Transform photon_transform)
	{
		GameObject result = GameObject.Instantiate (this.photon, photon_transform.position, photon_transform.rotation);
		result.transform.parent = EmptyGO.transform;

		return result;
	}

	//-----------------------------------------------------------------------------------//

	private GameObject InstantiatePhoton_laserCavity_noRB(Transform photon_transform)
	{
		GameObject result1 = GameObject.Instantiate (this.photon_noRB, photon_transform.position, photon_transform.rotation);
		result1.transform.parent = EmptyGO.transform;

		return result1;
	}
}



