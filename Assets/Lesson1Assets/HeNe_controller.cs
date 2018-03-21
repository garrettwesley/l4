using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeNe_controller : MonoBehaviour {


	public Spontaneous_emission sponEm;
	public Animator Ne_anim;
	public Animator HeNeController_translate;
	public Animator HENeConroller_excitation;

	public bool shouldPhotonSpawn;
	public bool shouldAnimationLoop;

	int NeTrigger = Animator.StringToHash("Ne_excite");
	int HeNeTrigger = Animator.StringToHash("StartHeNe");
    int HeNeTrigger2 = Animator.StringToHash("StartHeNe");

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.T))
		{
             this.HeNeController_translate.SetBool (HeNeTrigger2, true);
             this.HENeConroller_excitation.SetBool (HeNeTrigger2, true);
            // this.HeNeController_translate.SetTrigger (HeNeTrigger);
            // this.HENeConroller_excitation.SetTrigger (HeNeTrigger);
        }

    }


	public void StartHeNeEnergyTransfer ()
	{
		this.HeNeController_translate.SetBool (HeNeTrigger2, true);
		this.HENeConroller_excitation.SetBool (HeNeTrigger2, true);

	}

	public void ExciteNe()
	{
		this.Ne_anim.SetBool (NeTrigger, true);	
	}

	public void DestroyPhoton ()
	{
		this.sponEm.DestroyPhoton ();

	}

	public void SpawnPhoton()
	{
		
		if (shouldPhotonSpawn == true)
		{
			sponEm.SpawnPhoton_sponEm ();

		}

	

    }

	public void HeReset()
	{
		this.Ne_anim.SetBool (NeTrigger, false);
		this.HeNeController_translate.SetBool(HeNeTrigger2, false);
		this.HENeConroller_excitation.SetBool(HeNeTrigger2, false);

	}

	public void NeExcitation()
	{
		Ne_anim.SetBool (NeTrigger, true);

	}

	public void NeReset()
	{
//		Ne_anim.SetBool (NeTrigger, false);
		if(shouldAnimationLoop == true)
		{
			StartHeNeEnergyTransfer ();
		}
	}

}
