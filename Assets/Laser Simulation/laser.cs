using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class laser_test : MonoBehaviour {


	private GameObject[] electrons_e0;
	private GameObject[] electrons_e1;
	private GameObject[] electrons_e2;

	private GameObject[] laser_electron;

	private Renderer[] rend_e0;
	private Renderer[] rend_e1;
	private Renderer[] rend_e2;

	private Renderer[] laserElectronRend;

	private List<int> counterList = new List<int>();
	private int pickedE;
	private int pickedIndex;





	public int e0 = 20;
	public int e1;
	public int e2;
	public float delayExite;
	public float delay_e2toe1;
	public float delay_e1toe0;

	public bool allowExcite;
	public bool relaxtoE1;
	public bool relaxtoE0;

	public int NumParticle;
	public Transform origin_e0;
	public Transform origin_e1;
	public Transform origin_e2;

	public float offset;
	public GameObject electronGO;

	public GameObject E0_group;
	public GameObject E1_group;
	public GameObject E2_group;
	public GameObject E_laserGroup;

    public float Mirror_Reflectivity;

    //Sliders for energy level lifetimes and mirror reflectivity
    public Slider E2_Lifetime;
    public Slider E1_LifeTime;
    public Slider Mirror_Reflectivity_Slider;

	public Color originalColor = Color.white;



	// Use this for initialization
	void Start () {

        //Gets the value of each slider and sets the proper exitation delay variable to said value
        delay_e2toe1 = E2_Lifetime.value;
        delay_e1toe0 = E1_LifeTime.value;
        Mirror_Reflectivity = Mirror_Reflectivity_Slider.value;

		electrons_e0 = new GameObject[NumParticle];
		electrons_e1 = new GameObject[NumParticle];
		electrons_e2 = new GameObject[NumParticle];
		laser_electron = new GameObject[NumParticle];

		for (int i=0; i< NumParticle; i++)
		{
			this.electrons_e0 [i] = GameObject.Instantiate (electronGO, origin_e0.position - new Vector3 (i, 0, 0) * offset, origin_e0.rotation);
			this.electrons_e1 [i] = GameObject.Instantiate (electronGO, origin_e1.position - new Vector3 (NumParticle-1 * offset,0,0) + new Vector3 (i, 0, 0) * offset, origin_e1.rotation);
			this.electrons_e2 [i] = GameObject.Instantiate (electronGO, origin_e2.position - new Vector3 (NumParticle-1 * offset,0,0) + new Vector3 (i, 0, 0) * offset, origin_e2.rotation);

			this.laser_electron[i] = GameObject.Instantiate (electronGO, origin_e2.position - new Vector3 (NumParticle-1 * offset,0,0) + new Vector3 (1.5f* i, -10, 0) * offset, origin_e2.rotation);


			this.electrons_e0 [i].transform.parent = this.E0_group.transform;
			this.electrons_e1 [i].transform.parent = this.E1_group.transform;
			this.electrons_e2 [i].transform.parent = this.E2_group.transform;
			this.laser_electron[i].transform.parent = this.E_laserGroup.transform;



		}

		rend_e0 = new Renderer[NumParticle];
		rend_e1 = new Renderer[NumParticle];
		rend_e2 = new Renderer[NumParticle];

		laserElectronRend = new Renderer[NumParticle];

		for (int i=0; i< NumParticle; i++)
		{
			rend_e0[i] = electrons_e0[i].GetComponent<Renderer>();
			rend_e1[i] = electrons_e1[i].GetComponent<Renderer>();
			rend_e2[i] = electrons_e2[i].GetComponent<Renderer>();

			laserElectronRend[i] = laser_electron[i].GetComponent<Renderer>();
		}


		for (int i = 0; i < NumParticle; i++)
		{
			counterList.Add (i);

		}



		StartCoroutine (ExcitationGate ());
//		StartCoroutine (E1Gate ());
//		StartCoroutine (E0Gate ());



	}
	
	// Update is called once per frame
	void Update () {

        //Sets the delay parameters to their current slider value
        delay_e2toe1 = E2_Lifetime.value;
        delay_e1toe0 = E1_LifeTime.value;
        Mirror_Reflectivity = Mirror_Reflectivity_Slider.value;

        //Sets Delay_e 2toe1 and Delay_e 1toe0

        if (allowExcite == true  && e0>0  && e2 < NumParticle)
		{
			e0--;
			e2++;
			pickedE = counterList [Random.Range (0, counterList.Count)];// Changed second paramter of Random.range(int x, int y) too counterList.Count from NumParticle, this avoids index out of bounds error
//			pickedIndex = System.Array.IndexOf (counterList, pickedE);
			counterList.Remove(pickedE);

			StartCoroutine (Relax ());
			allowExcite = false;
			StartCoroutine (ExcitationGate ());

		}

//		if (relaxtoE1 == true && e2>0  && e1 < NumParticle)
//		{
//			e2--;
//			e1++;
//			relaxtoE1 = false;
//			StartCoroutine (E1Gate ());
//
//		}
//
//		if (relaxtoE0 == true && e1>0 && e0 < NumParticle)
//		{
////			if (e1 == 0)
////			{
////				relaxtoE0 = false;
////				StartCoroutine (E0Gate ());
////			}
//			e1--;
//			e0++;
//			relaxtoE0 = false;
//			StartCoroutine (E0Gate ());
//

//		}

		Debug.Log ("E0 = " + e0 + ", E1 = " + e1 + ", E2 = " + e2);


		for (int i = 0; i < NumParticle; i++)
		{
			rend_e0 [i].GetComponent<MeshRenderer> ().enabled = true;
		}

		for (int i = 0; i < NumParticle - e0; i++)
		{
			rend_e0 [i].GetComponent<MeshRenderer> ().enabled = false;
		}

		for (int i = 0; i < NumParticle; i++)
		{
			rend_e1 [i].GetComponent<MeshRenderer> ().enabled = false;
		}

		for (int i = 0; i <  e1; i++)
		{
			rend_e1 [i].GetComponent<MeshRenderer> ().enabled = true;
		}

		for (int i = 0; i < NumParticle; i++)
		{
			rend_e2 [i].GetComponent<MeshRenderer> ().enabled = false;
		}

		for (int i = 0; i <  e2; i++)
		{
			rend_e2 [i].GetComponent<MeshRenderer> ().enabled = true;
		}



	}

	public IEnumerator ExcitationGate()
	{
		yield return new WaitForSeconds (delayExite);


		allowExcite = true;

	}

	public IEnumerator Relax()
	{
		laserElectronRend [pickedE].material.color = Color.blue;


		yield return new WaitForSeconds (delay_e2toe1);
		e2--;
		e1++;

		laserElectronRend [pickedE].material.color = Color.green;

		yield return new WaitForSeconds (delay_e1toe0);
		e1--;
		e0++;
		laserElectronRend [pickedE].material.color = originalColor;
		counterList.Add(pickedE);



	}


	public IEnumerator E0Gate()
	{
		yield return new WaitForSeconds (delay_e1toe0);
		relaxtoE0 = true;

	}


}
