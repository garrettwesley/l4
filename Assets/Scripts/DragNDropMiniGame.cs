using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;


public class DragNDropMiniGame : MonoBehaviour
{
    private Material originalMaterial;
	private GameObject[] currentObject = new GameObject[5];
	private Vector3[] instantiatedPosition = new Vector3[5];
	private int LaserPartIndex;
	private GameObject[] DroppedLaserParts;
	private bool LaserAssembled;
	private bool gameComplete;
	private bool cursorVisible;
	private Killvolume kv;
	private reloadSpaceship rs;
	private PauseMenu pauseMenu;




	public Button[] yourButton;
    public Material HoverMaterial;
    public Camera Camera;
    public float SnapTolerance = 0.5f;
	public GameObject[] LaserParts = new GameObject[4];
    public GameObject[] SnapPointPhysicalParts = new GameObject[5];
    public GameObject Wall;
    public Transform[] SnapPoints;
	public GameObject LaserBeam;
	public GameObject ReflectionProbe;
	public Animator Anim_BtnActive;
	public Camera camFPC;
	public FirstPersonController FirstPersonController;
	public GameObject Minigame_gameObject;
	public GameObject halo;
	public GameObject FullLaser_inHolodeck;
	public GameObject doorWaySphere;
	public GameObject doorWayPlane;
	public Material laserBeamRed;
	public Material doorWaymaterial;
	public Text RedOrbText;
	public GameObject OrbHUD;
    public GameObject Lesson3;
    public GameObject Lesson4;
    public GameObject Lesson5;
    public GameObject Lesson6;



	int LaserActive = Animator.StringToHash("LaserActive");


	void OnTriggerEnter(Collider other)
	{
		if (gameComplete == false)
		{
            for (int i = 0; i < 5; i++)//Reveals snap points and such too player to allow them to play the game
            {
                SnapPointPhysicalParts[i].SetActive(true);
            }
            Wall.SetActive(true);

            Lesson3.SetActive(false);// Deactivates the lessons that lay behind the minigame
            Lesson4.SetActive(false);
            Lesson5.SetActive(false);
            Lesson6.SetActive(false);

            halo.SetActive(false);
            this.Camera.enabled = true;
			this.camFPC.enabled = false;
			this.FirstPersonController.GetComponent<FirstPersonController>().enabled = false;
			this.Minigame_gameObject.SetActive(true);
			OrbHUD.SetActive (false);
			pauseMenu.pauseMenuAccessible = false;



			cursorVisible = true;
		}


	}



    // ------------------------------------------------------------------------------------- //

    public void Start()
    { 
		pauseMenu = GameObject.Find ("PauseMenu").GetComponent<PauseMenu> ();
		kv = GameObject.Find ("KillVolume").GetComponent<Killvolume> ();
		rs = GameObject.Find ("LevelLoader").GetComponent<reloadSpaceship> ();
	}

    // ------------------------------------------------------------------------------------- //

    private Transform GetClosestSnap()
    {
		if (this.currentObject[LaserPartIndex] == null)
        {
            return null;
        }

        float minDistance = float.MaxValue;
        Transform winner = null;

        foreach (Transform t in this.SnapPoints)
        {
			float distance = (t.position - this.currentObject[LaserPartIndex].transform.position).magnitude;
            if (distance < this.SnapTolerance && distance < minDistance)
            {
                minDistance = distance;
                winner = t;
            }
        }
        return winner;
    }

    // ------------------------------------------------------------------------------------- //

    public void Update()
    {

		if (cursorVisible == true)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

        Transform winner = GetClosestSnap();
        if (winner != null)
        {
			this.currentObject[LaserPartIndex].transform.position = winner.transform.position;
			instantiatedPosition [LaserPartIndex] = this.currentObject[LaserPartIndex].transform.position;
        }

		if (SnapPoints[0].position == instantiatedPosition[0]
			&& SnapPoints[1].position == instantiatedPosition[1]
			&& SnapPoints[2].position == instantiatedPosition[2]
			&& SnapPoints[3].position == instantiatedPosition[3]
			&& SnapPoints[4].position == instantiatedPosition[4]
		)
			{
				this.Anim_BtnActive.SetBool (LaserActive, true);
				LaserAssembled = true;
			}
    }

    // ------------------------------------------------------------------------------------- //

	private void UpdateCurrentObject(GameObject t, PointerEventData pointerData)
    {
		if (this.currentObject[LaserPartIndex] == null)
        {
//			this.currentObject = this.DroppedLaserParts [LaserPartIndex] = GameObject.Instantiate (t);
			this.currentObject[LaserPartIndex] = GameObject.Instantiate (t);
			Debug.Log (this.currentObject [LaserPartIndex]);

			this.currentObject[LaserPartIndex].gameObject.GetComponent<Collider>().enabled = false;
			this.originalMaterial = this.currentObject[LaserPartIndex].GetComponent<Renderer>().material;
			this.currentObject[LaserPartIndex].GetComponent<Renderer>().material = this.HoverMaterial;
//			this.currentObject = null;

        }


        var ray = this.Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
			this.currentObject[LaserPartIndex].transform.position = hit.point;

        }

	}

	// ------------------------------------------------------------------------------------- //

	public void OnDrag_MirrorPartial(BaseEventData data)
	{
		LaserPartIndex = 0;
		UpdateCurrentObject(this.LaserParts[0], data as PointerEventData);
	}

    // ------------------------------------------------------------------------------------- //

    public void OnDrag_LaserBody(BaseEventData data)
    {
		LaserPartIndex = 1;
		UpdateCurrentObject(this.LaserParts[1], data as PointerEventData);
    }

    // ------------------------------------------------------------------------------------- //

	public void OnDrag_Wires(BaseEventData data)
	{
		LaserPartIndex = 2;
		UpdateCurrentObject(this.LaserParts[2], data as PointerEventData);


	}

	// ------------------------------------------------------------------------------------- //

    public void OnDrag_MirrorFull(BaseEventData data)
    {
		LaserPartIndex = 3;
		UpdateCurrentObject(this.LaserParts[3], data as PointerEventData);


    }

    // ------------------------------------------------------------------------------------- //



	public void OnDrag_Battery(BaseEventData data)
	{
		LaserPartIndex = 4;
		UpdateCurrentObject(this.LaserParts[4], data as PointerEventData);


	}

    // ------------------------------------------------------------------------------------- //



    public void OnPointerUp(BaseEventData data)
    {
        Transform winner = GetClosestSnap();
        if (winner != null)
        {
			this.currentObject[LaserPartIndex].GetComponent<Renderer>().material = this.originalMaterial;
//			this.currentObject[LaserPartIndex] = null;
        }
        else
        {
			GameObject.Destroy(this.currentObject [LaserPartIndex]);
        }
    }

    // ------------------------------------------------------------------------------------- //

	public void OnClick()
	{
		Button btn_clear = yourButton[0].GetComponent<Button>();
		Button btn_activate = yourButton[1].GetComponent<Button>();

		btn_clear.onClick.AddListener(TaskOnClick_clear);
		btn_activate.onClick.AddListener (TaskonClick_activate);


	}

	public void TaskOnClick_clear()
	{
		Debug.Log ("test");
		Debug.Log (this.currentObject [0]);

		if (this.currentObject[0] != null)
		{
			Destroy (this.currentObject [0]);
		}

		if (this.currentObject[1] != null)
		{
			Destroy (this.currentObject [1]);
		}

		if (this.currentObject[2] != null)
		{
			Destroy (this.currentObject [2]);
		}

		if (this.currentObject[3] != null)
		{
			Destroy (this.currentObject [3]);
		}

		if (this.currentObject[4] != null)
		{
			Destroy (this.currentObject [4]);
		}

		for (int i=0; i < instantiatedPosition.Length; i++)
		{
			instantiatedPosition [i] = Vector3.zero;
		}	


	}

	public void TaskonClick_activate()
	{
		if (LaserAssembled == true)
		{
			this.LaserBeam.SetActive (true);
			this.ReflectionProbe.SetActive (true);
			StartCoroutine (EndSequence ());
		}

	}

	public IEnumerator EndSequence ()
	{
		yield return new WaitForSeconds (0.5f);
		ReturnToFPC ();
	}

	public void ReturnToFPC()
	{
        for (int i = 0; i < 5; i++)//Hides snap points and wall from player
        {
            SnapPointPhysicalParts[i].SetActive(false);
        }
        TaskOnClick_clear();
        Wall.SetActive(false);

        Lesson3.SetActive(true);// Reactivates Lessons that lay behind the minigame
        Lesson4.SetActive(true);
        Lesson5.SetActive(true);
        Lesson6.SetActive(true);

        FullLaser_inHolodeck.SetActive(true);

        this.halo.SetActive (false);
		this.Camera.enabled = false;
		this.camFPC.enabled = true;
		this.FirstPersonController.GetComponent<FirstPersonController>().enabled = true;
		this.Minigame_gameObject.SetActive(false);
		this.cursorVisible = false;
		this.OrbHUD.SetActive (true);
		pauseMenu.pauseMenuAccessible = true;


		if(RedOrbText.text == "1 / 2")
		{
			this.gameComplete = true;
			kv.spawnNum = 7;
			kv.RespawnPlayer ();
			this.RedOrbText.text = "2 / 2";

			//FullLaser_inHolodeck.SetActive (true);
			doorWaySphere.GetComponent<Renderer> ().material = laserBeamRed;
			doorWayPlane.SetActive (true);
			rs.HolodeckComplete = true;
		}

		if(RedOrbText.text == "0 / 2")
		{
			this.gameComplete = true;
			kv.spawnNum = 7;
			kv.RespawnPlayer ();
			this.RedOrbText.text = "1 / 2";
			//FullLaser_inHolodeck.SetActive (true);


		}
			




	}

	public void ReturnToFPCviaExit()
	{
        for (int i = 0; i < 5; i++)//Hides snap points and wall from player
        {
            SnapPointPhysicalParts[i].SetActive(false);
        }
        TaskOnClick_clear();
        Wall.SetActive(false);

        Lesson3.SetActive(true);// Reactivates Lessons that lay behind the minigame
        Lesson4.SetActive(true);
        Lesson5.SetActive(true);
        Lesson6.SetActive(true);

        halo.SetActive(true);
        this.Camera.enabled = false;
		this.camFPC.enabled = true;
		this.Minigame_gameObject.SetActive(false);
		this.FirstPersonController.GetComponent<FirstPersonController>().enabled = true;
		this.cursorVisible = false;
		this.OrbHUD.SetActive (true);
		pauseMenu.pauseMenuAccessible = true;

	}

	public void ActivateDoorWay()
	{
		//FullLaser_inHolodeck.SetActive (true);
		doorWaySphere.GetComponent<Renderer> ().material = laserBeamRed;
		doorWayPlane.SetActive (true);
		rs.HolodeckComplete = true;

	}

    public bool getGameComplete()
    {
        return gameComplete;
    }

}
