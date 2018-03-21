using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class macroAnimTrigger : MonoBehaviour {


	public HeliumAtom heAtom;
	public FiredPhoton fPhoton;
	public HeNe_controller HeNe;
	public Energy3LevelController e3l;

	public bool HeliumAtomTrigger;
	public bool SponEmissionTrigger;
	public bool HeNeTrigger;


	void Start () {
		
	}
	
	void Update () {
		
	}

	public void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player" && HeliumAtomTrigger == true)
		{
			heAtom.Excite ();
			fPhoton.Loop = true;
		}

		if(other.tag == "Player" && SponEmissionTrigger == true)
		{
			heAtom.Excite ();
		}
	
		if(other.tag == "Player"  && HeNeTrigger == true)
		{
			HeNe.StartHeNeEnergyTransfer ();
			e3l.Excite ();
		
		}
	}

}
