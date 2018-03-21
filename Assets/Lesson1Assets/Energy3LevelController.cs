using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy3LevelController : MonoBehaviour {


	private GameObject photon;

	public Animator e3l;
	public Spontaneous_emission sponEm;
	public bool shouldAnimationLoop;

	int eUp = Animator.StringToHash("Electron_ED_up");
	int eDown = Animator.StringToHash("Electron_ED_down");
	int eHe2Ne = Animator.StringToHash("Electron_He2Ne");
	int interNe = Animator.StringToHash("Electron_interNe");


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.T))
		{
			ElectronReset ();
		}
	}

	public void Excite()
	{
		e3l.SetTrigger (eUp);
	}

	public void Transfer()
	{
		e3l.SetTrigger (eHe2Ne);
	}

	public void Relax()
	{
		e3l.SetTrigger (interNe);
	}

	public void ElectronReset()
	{
		e3l.SetTrigger (eDown);
	}


	public void SpawnPhoton()
	{
		sponEm.SpawnPhoton_sponEm();
	}

	public void DestroyPhoton()
	{
		this.sponEm.DestroyPhoton ();
	}

	public void RestartAnimation()
	{
		if(shouldAnimationLoop == true)
		{
			Excite ();
		}
	}

}
