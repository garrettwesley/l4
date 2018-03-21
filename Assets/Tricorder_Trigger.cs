using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tricorder_Trigger : MonoBehaviour {

	private ObjectivesUI ui;
	private EngineLaserController elc;
	private bool pickUpTricorder;

	public GameObject canvas;
	public GameObject halo;




	void Start () {

		ui = GameObject.Find ("ObjectivesUI").GetComponent<ObjectivesUI> ();
		elc = GameObject.Find ("EngineLaser").GetComponent<EngineLaserController> ();
		this.halo.SetActive (false);
	}

	void Update () {
		
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && pickUpTricorder == true) 
		{
			StartCoroutine (tricorderHUD());  //coroutine ends in destorying game object - must be called last

		}
	}

	public void ActivateTricorderPickup()
	{
		this.halo.SetActive (true);
		pickUpTricorder = true;

	}


	public IEnumerator tricorderHUD ()
	{
		this.gameObject.GetComponent<Renderer> ().enabled = false;
		this.halo.SetActive (false);
		this.canvas.SetActive(true);
		yield return new WaitForSeconds (1f);
		ui.AdvanceObjective ();
		elc.tricorderCollected = true;
		yield return new WaitForSeconds (2f);
		this.canvas.SetActive(false);
		Destroy (this.gameObject);

	}
}
