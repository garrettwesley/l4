using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LaserMiniGame : MonoBehaviour
{
    private enum Stage
    {
        SingleExcitation,
        DoubleExcitation,
    }

    private int secondStageElectronsRemaining = 2;
    private Stage currentStage;
    private GameObject lastHeliumHovered;
    private GameObject helium1;
    private GameObject helium2;
    private bool cursorVisible;
	private bool gameComplete;
	private bool doublePhotonSpawned;
	private bool gameactive;
	private PauseMenu pauseMenu;
	private int spawnCount;


	public GameObject minigameUI;
	public Text instructions;
    public GameObject HeliumAtom;
    public Camera Camera;
    public GameObject ElectronReceiver;
    public Camera camFPC;
    public FirstPersonController FirstPersonController;
    public GameObject Minigame_gameObject;
	public GameObject halo;
	public Text RedOrbText;
	public GameObject OrbHUD;
	public GameObject UIhelp;
	public GameObject wall;
	public GameObject Helium2Parent;



    void OnTriggerEnter(Collider other)
    {
		if (gameComplete == false)
		{
			this.gameactive = true;
			this.Camera.enabled = true;
			this.camFPC.enabled = false;
			this.FirstPersonController.GetComponent<FirstPersonController>().enabled = false;
			this.Minigame_gameObject.SetActive(true);
			OrbHUD.SetActive (false);

			this.currentStage = Stage.SingleExcitation;
			this.helium1 = InstantiateHelium();
			cursorVisible = true;
			this.minigameUI.SetActive (true);
			pauseMenu.pauseMenuAccessible = false;

		}
     

    }


    // ------------------------------------------------------------------------------------- //

    public void Start()
    { 
//		this.currentStage = Stage.SingleExcitation;
		pauseMenu = GameObject.Find ("PauseMenu").GetComponent<PauseMenu> ();


	}

    // ------------------------------------------------------------------------------------- //

    private GameObject InstantiateHelium()
    {
        GameObject result = GameObject.Instantiate(this.HeliumAtom);
        HeliumAtom heliumAtom = result.GetComponent<HeliumAtom>();
        heliumAtom.Camera = this.Camera;
		Debug.Log ("CreateHelium");
        return result;
    }

    // ------------------------------------------------------------------------------------- //

    public void Update()
    {

        if (cursorVisible == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

		if (gameactive == true)
		{
			if (Input.GetKeyDown(KeyCode.Space) && this.lastHeliumHovered != null)
			{
				switch (this.currentStage)
				{
				case Stage.DoubleExcitation:
					this.secondStageElectronsRemaining = 2;
					break;
				}

				this.lastHeliumHovered.GetComponent<HeliumAtom>().Excite();
			}

			var ray = Camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider.transform.gameObject.name == "AtomCollider")
				{
					this.lastHeliumHovered = hit.collider.transform.parent.gameObject;
				}
			}

		}

		if(helium2 == null){
			return;}
		if(helium2.GetComponent<HeliumAtom> ().UIbuttonClickCounter == 3  )
		{
			UIhelp.SetActive (true);
		}

		if (spawnCount == 2)
		{
			foreach (Transform child in Helium2Parent.transform)
			{
				GameObject.Destroy (child.gameObject);
			}
			this.helium2 = InstantiateHelium();
			this.helium2.transform.position = new Vector3(232.95f, 0f, -24.42f);

			spawnCount = 0;

		}
        
    }

    // ------------------------------------------------------------------------------------- //

    public void AdvanceToDoubleExcitation1()
    {
        this.currentStage = Stage.DoubleExcitation;
        this.helium2 = InstantiateHelium();
		this.helium2.transform.position = new Vector3(232.95f, 0f, -24.42f);
		this.helium2.transform.parent = this.Helium2Parent.transform;
		this.instructions.text = "Shoot 2 Photons into the Collector";
    }

    // ------------------------------------------------------------------------------------- //

    public void HandleElectronCollision(FiredPhoton electron, Collider objectCollidedWith)
    {
        switch (this.currentStage)
        {
		case Stage.SingleExcitation:
			Debug.Log ("Collision");
			StartCoroutine (StageTransition ());
//			AdvanceToDoubleExcitation1();
			    
			break;
            case Stage.DoubleExcitation:
                if (objectCollidedWith.gameObject == this.ElectronReceiver)
                {
				if (/*--this.secondStageElectronsRemaining == 0*/ doublePhotonSpawned == true )
                    {
//                        var splines = GameObject.FindObjectsOfType<CatmullRomSpline>();
//                        foreach (var spline in splines)
//                        {
//                            spline.enabled = false;
//                        }
//                        var nucleusRotations = GameObject.FindObjectsOfType<NucleusRotation>();
//                        foreach (var nucleusRotation in nucleusRotations)
//                        {
//                            nucleusRotation.enabled = false;
//                        }
//                        //					SceneManager.LoadScene("Holodeck2");
					StartCoroutine(TransitiontoExit());

                    }
                }
                else
                {
                    if (objectCollidedWith.gameObject.transform.parent.gameObject == this.helium2)
                    {
                        Excitation e = this.helium2.GetComponent<HeliumAtom>().Excitation.GetComponent<Excitation>();
                        if (e == null)
                        {
                            return;
                        }
                        print(e.CurrentPercentage);
                        if (e.CurrentPercentage > 0.65f)
                        {
                            e.SpawnSecondPhoton = true;
						this.doublePhotonSpawned = true;
                        }
                    }
                }
                break;
        }
    }

	public IEnumerator StageTransition()
	{
		this.instructions.text = "Great Job!!!";
		print ("AdvanceStage");
		yield return new WaitForSeconds (3f);
		AdvanceToDoubleExcitation1();
		spawnCount++;
		yield break;
		
	}

	public IEnumerator TransitiontoExit()
	{
		this.instructions.text = "Great Job!!!";
		yield return new WaitForSeconds (3f);
		ReturnToFPC();
	}

	// ------------------------------------------------------------------------------------- //


	public IEnumerator DoublePhotonTimer()
	{
		yield return new WaitForSeconds (2f);
		this.doublePhotonSpawned = false;

	}

    // ------------------------------------------------------------------------------------- //
    public void ReturnToFPC()
    {
		this.helium1.SetActive (false);
		if (helium2 != null){
			this.helium2.SetActive (false);
		}
		this.halo.SetActive (false);
        this.Camera.enabled = false;
        this.camFPC.enabled = true;
        this.FirstPersonController.GetComponent<FirstPersonController>().enabled = true;
        this.Minigame_gameObject.SetActive(false);
        this.cursorVisible = false;
		this.gameComplete = true;
		this.minigameUI.SetActive (false);
		this.OrbHUD.SetActive (true);
		this.RedOrbText.text = "1 / 2";
		this.wall.GetComponent<wallMove> ().openWall = true;
		pauseMenu.pauseMenuAccessible = true;



    }
    // ------------------------------------------------------------------------------------- //


	public void ReturnToFPCviaExit()
	{
		Destroy (helium1);
//		this.helium1.SetActive (false);
		if (helium2 != null){
			Destroy (helium2);
		}
		this.Camera.enabled = false;
		this.camFPC.enabled = true;
		this.FirstPersonController.GetComponent<FirstPersonController>().enabled = true;
		this.cursorVisible = false;
		this.minigameUI.SetActive (false);
		this.OrbHUD.SetActive (true);
		this.currentStage = Stage.SingleExcitation;
		pauseMenu.pauseMenuAccessible = true;



	}


}

