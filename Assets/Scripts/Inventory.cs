using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    private Material originalMaterial;
	private GameObject[] currentObject = new GameObject[5];
	private Vector3[] instantiatedPosition = new Vector3[5];
	private int LaserPartIndex;
	private GameObject[] DroppedLaserParts;
	private bool LaserAssembled;

	public LightmapData[] LightMapArray;
	public LightmapData mapdata = new LightmapData ();
	public Button[] yourButton;

    public Material HoverMaterial;
    public Camera Camera;
    public float SnapTolerance = 0.5f;
	public GameObject[] LaserParts = new GameObject[4];
    public Transform[] SnapPoints;
	public GameObject LaserBeam;
	public GameObject ReflectionProbe;
	public Animator Anim_BtnActive;

	int LaserActive = Animator.StringToHash("LaserActive");



    // ------------------------------------------------------------------------------------- //

    public void Start()
    { 
		LightmapData[] LightMapArray = LightmapSettings.lightmaps;

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

			


	}

	public void TaskonClick_activate()
	{
		if (LaserAssembled == true)
		{
			this.LaserBeam.SetActive (true);
			this.ReflectionProbe.SetActive (true);
		}

	}

}
