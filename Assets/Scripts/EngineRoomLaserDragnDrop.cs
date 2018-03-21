using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.EventSystems;

public class EngineRoomLaserDragnDrop : MonoBehaviour
{
	private LaserPartsPickupController lppc;
    private Material originalMaterial;
	private GameObject[] currentObject = new GameObject[5];
	private Vector3[] instantiatedPosition = new Vector3[5];
	private int LaserPartIndex;
	private GameObject[] DroppedLaserParts;
	private bool LaserAssembled;
	private bool controllerActive;
	private bool doneOnce;
	private PauseMenu pauseMenu;


	private FirstPersonController FPC;
	private Camera FPcam;
	private Camera Benchcam;
	private EngineLaserController elc;



	public Button[] yourButton;
    public Material HoverMaterial;
    public Camera Camera;
    public float SnapTolerance = 0.5f;
	public GameObject[] LaserParts = new GameObject[4];
    public Transform[] SnapPoints;
	public Light[] lights;
	public GameObject CompletedLaser;


	int LaserActive = Animator.StringToHash("LaserActive");
	int haloOn = Animator.StringToHash("haloOn");

	ObjectivesUI objUI;





    // ------------------------------------------------------------------------------------- //

    public void Start()
    { 
		pauseMenu = GameObject.Find ("PauseMenu").GetComponent<PauseMenu> ();
		FPC = GameObject.Find ("FPSController").GetComponent<FirstPersonController>();
		FPcam = GameObject.Find ("FirstPersonCharacter").GetComponent<Camera> ();
		Benchcam = this.gameObject.GetComponentInChildren<Camera> ();
		lppc = GameObject.Find ("LaserPartsHunt").GetComponent<LaserPartsPickupController> ();
		elc = GameObject.Find ("EngineLaser").GetComponent<EngineLaserController> ();
		objUI = GameObject.Find ("ObjectivesUI").GetComponent<ObjectivesUI> ();



	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && lppc.LaserPartsFound == true && LaserAssembled == false)
		{
			lppc.ResetHUD ();
			FPC.enabled = false;
			FPcam.enabled = false;
			Benchcam.enabled = true;
			controllerActive = true;
			yourButton [0].gameObject.SetActive (true);
			yourButton [1].gameObject.SetActive (true);
			pauseMenu.pauseMenuAccessible = false;

			for (int i=0; i < SnapPoints.Length; i++)
			{
				SnapPoints [i].gameObject.SetActive (true);

			}


		}

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

		if (controllerActive == true)
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
			&& SnapPoints[1].position == instantiatedPosition[3] && doneOnce == false
		)
			{
				elc.laserBuilt = true;
			StartCoroutine(ExitSequence());
			doneOnce = true;
				
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

//			this.currentObject[LaserPartIndex].gameObject.GetComponent<Collider>().enabled = false;
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
		LaserPartIndex = 2;
		UpdateCurrentObject(this.LaserParts[0], data as PointerEventData);
	}

    // ------------------------------------------------------------------------------------- //

    public void OnDrag_LaserBody(BaseEventData data)
    {
		LaserPartIndex = 1;
		UpdateCurrentObject(this.LaserParts[1], data as PointerEventData);
		print ("test");
    }

    // ------------------------------------------------------------------------------------- //

	public void OnDrag_Battery(BaseEventData data)
	{
		LaserPartIndex = 3;
		UpdateCurrentObject(this.LaserParts[2], data as PointerEventData);


	}

	// ------------------------------------------------------------------------------------- //

    public void OnDrag_MirrorFull(BaseEventData data)
    {
		LaserPartIndex = 0;
		UpdateCurrentObject(this.LaserParts[3], data as PointerEventData);


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



	public void TaskOnClick_clear()
	{


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

		for (int i=0; i < instantiatedPosition.Length; i++)
		{
			instantiatedPosition [i] = Vector3.zero;
		}	



	}

	public void TaskonClick_Exit()
	{
		FPC.enabled = true;
		FPcam.enabled = true;
		Benchcam.enabled = false;
		controllerActive = false;
		yourButton [0].gameObject.SetActive (false);
		yourButton [1].gameObject.SetActive (false);
		for (int i=0; i < SnapPoints.Length; i++)
		{
			SnapPoints [i].gameObject.SetActive (false);

		}
		objUI.AdvanceObjective ();
		pauseMenu.pauseMenuAccessible = true;

	}

	public IEnumerator ExitSequence()
	{
		for (int i = 0; i < lights.Length; i++)
		{
			lights [i].color = new Color32(0,200,0,255);
		}
		yield return new WaitForSeconds (2f);
		TaskonClick_Exit ();
		TaskOnClick_clear ();
		this.CompletedLaser.SetActive (true);
		LaserAssembled = true;
		for (int i=0; i < SnapPoints.Length; i++)
		{
			SnapPoints [i].gameObject.SetActive (false);
		}
		CompletedLaser.GetComponent<Animator> ().SetBool (haloOn, true);
		CompletedLaser.GetComponent<PickUpItem> ().HuntActivated = true;
		lppc.LaserPartsHUD.SetActive (false);
		pauseMenu.pauseMenuAccessible = true;

		yield break;


	}

}
