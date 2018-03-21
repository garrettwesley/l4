using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class laser : MonoBehaviour {


	private GameObject[] electrons_e0;
	private GameObject[] electrons_e1;
	private GameObject[] electrons_e2;

	private GameObject[] laser_electron;

	private Renderer[] rend_e0;
	private Renderer[] rend_e1;
	private Renderer[] rend_e2;

	private Renderer[] laserElectronRend;

    private Renderer[] electronRend;

	private List<int> counterList = new List<int>();
	private int pickedE;
	private int pickedIndex;

    private LinkedList<int> E0List;
    private LinkedList<int> E1List;
    private LinkedList<int> E2List;
    private LinkedList<int> Photon_List;

    private bool Mirror_Will_Fade;

    private float ProperR;//Proper floats are the RGB values of the Mirror
    private float ProperG;
    private float ProperB;
    private float BeamR;//Beam floats are the RGB values of the Laser Beam
    private float BeamG;
    private float BeamB;

    private GameObject[] Photons;
    private int Current_Number_Of_Photons_For_Start;//Just keeps track of photons as they are being initialized
    private int Current_Number_Of_Photons_For_Real;//Continuously keeps track of photons

    private int Num_Of_Collisions;
    private int Num_Of_Collisions_Allowed;

    private Destoyer_Of_Photons Photon_Destroyer_Script;

    private reloadSpaceship rs;

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
    public float View_Offset;//Controls rotation in the x direction of the display lines
	public GameObject electronGO;

	public GameObject E0_group;
	public GameObject E1_group;
	public GameObject E2_group;
	public GameObject E_laserGroup;

    public float Mirror_Reflectivity;
    public GameObject PartialMirror;
    public Material Mirror_Material;

    public GameObject[] electrons;

    //The photon model and the max number of photons;
    public GameObject photonGO;
    public int Max_Num_Of_Photons;

    //Sliders for energy level lifetimes and mirror reflectivity
    public Slider E2_Lifetime;
    public Slider E1_LifeTime;
    public Slider Mirror_Reflectivity_Slider;
    public Slider Current;

	public Color originalColor = Color.white;

    public Light Point_Light_1;
    public Light Point_Light_2;
    public GameObject Photon_Destroyer;

    public Image[] Laser_Output_Meter;

    //Laser beam
    public Material Laser_Beam;
    public Color Laser_Beam_Color;
    public Material Laser_Internal_Beam;

    //Doorway Back
    public Door_Controller DC;

    // Use this for initialization
    void Start ()
    {
        rs = GameObject.Find("LevelLoader").GetComponent<reloadSpaceship>();

        //Gets the color of the Laser Beam
        Laser_Beam_Color = Laser_Beam.color;
        BeamR = Laser_Beam_Color.r;
        BeamG = Laser_Beam_Color.g;
        BeamB = Laser_Beam_Color.b;

        //Gets the script of the main photon destroyer
        Photon_Destroyer_Script = Photon_Destroyer.GetComponent<Destoyer_Of_Photons>();

        //Sets the point light's intensity to 0;
        Point_Light_1.intensity = 0;
        Point_Light_2.intensity = 0;

        //Sets the current number of photons for both below paramters
        Current_Number_Of_Photons_For_Start = 0;
        Current_Number_Of_Photons_For_Real = 0;
        //Sets the max value of the Photons emitted
        Photons = new GameObject[Max_Num_Of_Photons];

        //Gets the RGB values of the Mirror so as not to change them when changing the transparency
        Color MirrorColor = Mirror_Material.color;
        ProperR = MirrorColor.r;
        ProperG = MirrorColor.g;
        ProperB = MirrorColor.b;

        //Gets the value of each slider and sets the proper exitation delay variable to said value
        delay_e2toe1 = E2_Lifetime.value;
        delay_e1toe0 = E1_LifeTime.value;
        Mirror_Reflectivity = Mirror_Reflectivity_Slider.value;
        delayExite = Current.value;

        //Initializing the three linked lists;
        E0List = new LinkedList<int>();
        E1List = new LinkedList<int>();
        E2List = new LinkedList<int>();
        Photon_List = new LinkedList<int>();

		electrons_e0 = new GameObject[NumParticle];
		electrons_e1 = new GameObject[NumParticle];
		electrons_e2 = new GameObject[NumParticle];
		//laser_electron = new GameObject[NumParticle];

		for (int i=0; i< NumParticle; i++)
		{
			this.electrons_e0 [i] = GameObject.Instantiate (electronGO, origin_e0.position + new Vector3 (i*View_Offset, 0, i) * offset, origin_e0.rotation);
			this.electrons_e1 [i] = GameObject.Instantiate (electronGO, origin_e1.position + new Vector3 (i*View_Offset, 0, i) * offset, origin_e1.rotation);
			this.electrons_e2 [i] = GameObject.Instantiate (electronGO, origin_e2.position + new Vector3 (i*View_Offset, 0, i) * offset, origin_e2.rotation);

			//this.laser_electron[i] = GameObject.Instantiate (electronGO, new Vector3(origin_e0.position.x + 0.05f, origin_e0.position.y, origin_e0.position.z) + new Vector3 (i*View_Offset, -1, i) * offset, origin_e2.rotation);


			this.electrons_e0 [i].transform.parent = this.E0_group.transform;
			this.electrons_e1 [i].transform.parent = this.E1_group.transform;
			this.electrons_e2 [i].transform.parent = this.E2_group.transform;
			//this.laser_electron[i].transform.parent = this.E_laserGroup.transform;

            int temp = Random.Range(0, NumParticle);
            while(E0List.Contains(temp))
            {
                temp = Random.Range(0, NumParticle);
            }
            E0List.AddLast(temp);
		}

		rend_e0 = new Renderer[NumParticle];
		rend_e1 = new Renderer[NumParticle];
		rend_e2 = new Renderer[NumParticle];

		//laserElectronRend = new Renderer[NumParticle];

        electronRend = new Renderer[NumParticle];

		for (int i=0; i< NumParticle; i++)
		{
			rend_e0[i] = electrons_e0[i].GetComponent<Renderer>();
			rend_e1[i] = electrons_e1[i].GetComponent<Renderer>();
			rend_e2[i] = electrons_e2[i].GetComponent<Renderer>();

			//laserElectronRend[i] = laser_electron[i].GetComponent<Renderer>();

            electronRend[i] = electrons[i].GetComponent<Renderer>();
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
	void Update ()
    {
        //Sets the lights intesity on scale of 0-1 based on the current number of photons within the laser cavity
        Current_Number_Of_Photons_For_Real = Current_Number_Of_Photons_For_Start - Photon_List.Count;
        float percentage = ((float)Current_Number_Of_Photons_For_Real / 100);
        Point_Light_1.intensity = percentage * 5;
        Point_Light_2.intensity = percentage * 5;

        

        //Determines if the mirror is to fade based on reflectivity
        Mirror_Material.SetColor("_Color", new Color(ProperR, ProperG, ProperB, Mirror_Reflectivity));

        //Sets the delay parameters to their current slider value
        delay_e2toe1 = E2_Lifetime.value;
        delay_e1toe0 = E1_LifeTime.value;
        Mirror_Reflectivity = Mirror_Reflectivity_Slider.value;
        delayExite = Current.value;

        if (allowExcite == true && e0 > 0 && e2 < NumParticle)
        {
            //Decrements the count of num of particles in Energy level 0 and increases the number of particles in Energy Level 2 by 1
            e0--;
            e2++;

            //Removes the first item in E0List and adds it to E2 list;
            E2List.AddLast(E0List.First.Value);
            pickedE = E0List.First.Value;
            E0List.RemoveFirst();


            //I comment out all lines in this manner with the comment lines right on their immidiate left, this is for if I need to return to this code in the future
            //pickedE = counterList [Random.Range (0, counterList.Count)];// Changed second paramter of Random.range(int x, int y) to counterList.Count instead of NumParticle, this avoids index out of bounds error
            //pickedIndex = System.Array.IndexOf (counterList, pickedE);
            //counterList.Remove(pickedE);

            StartCoroutine(Relax());
            allowExcite = false;
            StartCoroutine(ExcitationGate());

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

		//Debug.Log ("E0 = " + e0 + ", E1 = " + e1 + ", E2 = " + e2);
        
        //Sets the proper level of the photon energy meter based on the number of photons being destroyed per second by the photon destroyer
        int Photons_per_QuarterSecond = Photon_Destroyer_Script.getPhotonsDestroyedPerSecond();//The method name remains the same as original but it now works on a 1/4 second basis
        

        if(E2_Lifetime.value < 0.6 && Photons_per_QuarterSecond != 0)
        {
            Photons_per_QuarterSecond = Photons_per_QuarterSecond + 2;
            if (E2_Lifetime.value < 0.4)
            {
            Photons_per_QuarterSecond = Photons_per_QuarterSecond + 2;
                if (E2_Lifetime.value < 0.2)
                {
                    Photons_per_QuarterSecond = Photons_per_QuarterSecond + 2;
                }
            }
        }

        float A = ((float)Photons_per_QuarterSecond) / 10;
        //Sets the A value of the Laser Beam
        Laser_Beam.SetColor("_Color", new Color(BeamR, BeamG, BeamB, A));
        Laser_Internal_Beam.SetColor("_Color", new Color(255, 0, 0, A));

        if(Photons_per_QuarterSecond >= 8)
        {
            DC.ActivateDoorWay();
        }

        for (int i = 0; i < 11; i++)
        {
            Laser_Output_Meter[i].enabled = false;
        }

        for(int i = 0; i < Photons_per_QuarterSecond; i++)
        {
            Laser_Output_Meter[i].enabled = true;
        }

		for (int i = 0; i < NumParticle; i++)
		{
			rend_e0 [i].GetComponent<MeshRenderer> ().enabled = true;
		}

		for (int i = NumParticle-1; i >= e0; i--)
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
		//laserElectronRend [pickedE].material.color = Color.blue;
        electronRend[pickedE].material.color = Color.blue;

		yield return new WaitForSeconds (Random.Range(delay_e2toe1 - 1, delay_e2toe1 + 1));
		e2--;
		e1++;

        E1List.AddLast(E2List.First.Value);
        pickedE = E2List.First.Value;
        E2List.RemoveFirst();

        //laserElectronRend [pickedE].material.color = Color.green;
        electronRend[pickedE].material.color = Color.green;

        //Emits a photon
        if (Current_Number_Of_Photons_For_Start < Max_Num_Of_Photons)
        {
            Photons[Current_Number_Of_Photons_For_Start] = GameObject.Instantiate(photonGO, electrons[pickedE].transform.position - new Vector3(0, 0, 0) * offset, electrons[pickedE].transform.rotation);
            Current_Number_Of_Photons_For_Start++;
        }
        
        else if(Current_Number_Of_Photons_For_Start == Max_Num_Of_Photons && Photon_List.Count != 0)
        {
            int temp = Photon_List.First.Value;
            Photon_List.RemoveFirst();
            Photons[temp] = GameObject.Instantiate(photonGO, electrons[pickedE].transform.position - new Vector3(0, 0, 0) * offset, electrons[pickedE].transform.rotation);
        }

        

        yield return new WaitForSeconds (Random.Range(delay_e1toe0 -1, delay_e1toe0 + 1));
		e1--;
		e0++;

        E0List.AddLast(E1List.First.Value);
        pickedE = E1List.First.Value;
        E1List.RemoveFirst();

        //laserElectronRend [pickedE].material.color = originalColor;
        electronRend[pickedE].material.color = originalColor;
        counterList.Add(pickedE);

    }


	public IEnumerator E0Gate()
	{
		yield return new WaitForSeconds (delay_e1toe0);
		relaxtoE0 = true;

	}

    public int getCurrentNumOfPhotons()
    {
        return Current_Number_Of_Photons_For_Real;
    }

    public GameObject getPhoton(int i)
    {
        if (Photon_List.Contains(i))
        {
            return null;
        }
        return Photons[i];
    }

    public int getPhotonI(GameObject x)
    {
        for(int i = 0; i < Current_Number_Of_Photons_For_Start; i++)
        {
            if(Photons[i] == x)
            {
                if(Photon_List.Contains(i))
                {
                    continue;
                }
                return i;
            }
        }
        //if the photon no longer exists
        return -1;
    }

    public void addToPhotonList(int i)
    {
        Photon_List.AddLast(i);
    }
}
