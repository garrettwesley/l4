using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesUI : MonoBehaviour
{
    public int currentObjectiveIndex;
	private int displayObjectes = Animator.StringToHash("DisplayObjectives");
	private int setAnimPosition = Animator.StringToHash("setAnimPosition");
	private int objAdvance = Animator.StringToHash("ObjAdvance");


	public RectTransform panel;
	public bool objectiveOpen = false;
    public string[] Objectives;
    public Animator ObjectivesUIAnimator;
    public Text UIText;
	public float objectiveCheckDelay = 0.01f;


	MasterControlScript MCS;
	BridgeController bc;



    // ------------------------------------------------------------------------------------- //

    public void Start()
    {
		this.UIText.text = this.Objectives[0];
		GameObject ddol = GameObject.Find("DontDestroyonLoad");
		MCS = ddol.gameObject.GetComponent<MasterControlScript>();
		bc = GameObject.Find ("BridgeController").GetComponent<BridgeController> ();


		if (MCS.backInDaShip == true)
		{
			currentObjectiveIndex = 3;
			this.UIText.text = this.Objectives[3];
			ShowUIonAdvance ();
			bc.intercom.clip = bc.intercomClip [3];
			bc.intercom.Play ();

		}

		if (MCS.reloadRedAlert == true)
		{
			currentObjectiveIndex = 4;
			this.UIText.text = this.Objectives[4];
			ShowUIonAdvance ();

		}
				
    }

    // ------------------------------------------------------------------------------------- //

    public void Update()
    {
		if (objectiveOpen == false && panel.anchoredPosition.y == 258.5) 
		{
			if (Input.GetKey (KeyCode.O)) 
			{
				EnableBoolInAnimator ();
				objectiveOpen = true;

//				StartCoroutine (DelayObj_open ());

			}
		}

		if (objectiveOpen == true && panel.anchoredPosition.y == 0) 
		{
			if (Input.GetKey (KeyCode.O)) 
			{
				DisableBoolInAnimator ();
				objectiveOpen = false;

//				StartCoroutine (DelayObj_close ());


			}
		}


//        if (Input.GetKeyDown(KeyCode.N))
//        {
//            AdvanceObjective();
//            return;
//        }
    }


	// ------------------------------------------------------------------------------------- //

	public void EnableBoolInAnimator()
	{
		this.ObjectivesUIAnimator.SetBool(setAnimPosition, true);
		this.ObjectivesUIAnimator.SetBool(displayObjectes, true);
	}


    // ------------------------------------------------------------------------------------- //

    public void DisableBoolInAnimator()
    {
		this.ObjectivesUIAnimator.SetBool(displayObjectes, false);
    }

	// ------------------------------------------------------------------------------------- //
		
	public void ShowUIonAdvance()
	{
		this.ObjectivesUIAnimator.SetBool(objAdvance, true);

	}

	// ------------------------------------------------------------------------------------- //

	public void RetractUI()
	{
		this.ObjectivesUIAnimator.SetBool(objAdvance, false);

	}


	// ------------------------------------------------------------------------------------- //

    public void AdvanceObjective()
    {
        this.UIText.text = this.Objectives[++this.currentObjectiveIndex % this.Objectives.Length];
		ShowUIonAdvance ();

    }

    // ------------------------------------------------------------------------------------- //

	public void ActivateHUD(bool text)
	{
		this.gameObject.SetActive (text);

	}


}
