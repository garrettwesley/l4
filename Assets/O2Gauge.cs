using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class O2Gauge : MonoBehaviour {


	private float progress = 1;

	public Image gauge;
	public Text percent;
	public float duration; // Duration of flashing red lights during red alert
	public float smoothness = 0.02f; 
	public float upMultiplier = 10f;

	private bool doOnce;
	private bool stoploop;

	MasterControlScript MCS;
	ObjectivesUI objUI;


	void Start () {

		MCS = GameObject.Find("DontDestroyonLoad").GetComponent<MasterControlScript>();
		objUI = GameObject.Find ("ObjectivesUI").GetComponent<ObjectivesUI> ();

	}
	
	void Update () {

//		if(Input.GetKeyDown(KeyCode.Y) && doOnce == false)
//		{
//			StartO2Counter ();
//			doOnce = true;
//		}
//
//
//		if(Input.GetKey(KeyCode.L))
//		{
//			StopCoroutine (O2Down ());
//			stoploop = true;
//			StartCoroutine (O2up ());
//		}

		if (progress < 0.01f)
		{
			MCS.reloadRedAlert = true;
			StopCoroutine (O2Down ());
			SceneManager.LoadScene ("spaceship_master2");
			MCS.backInDaShip = false;
			objUI.currentObjectiveIndex = 4;
			objUI.UIText.text = objUI.Objectives[4];
			progress = 1;

		}
		
	}

	public IEnumerator O2Down()
	{
		print ("down");
//		float progress = 1; //This float will serve as the 3rd parameter of the lerp function.
		float increment = smoothness/duration ; //The amount of change to apply.
		while(progress > 0)
		{
			
			gauge.fillAmount = progress;
			percent.text = ((progress * 100f).ToString ("##.#") + "%");

			if(progress > 0.5f)
			{
				float temp = Remap (progress, 0.5f, 1f, 0f, 1f);
				gauge.color = new Color (1-temp, 1f, 0f);

			}

			if(progress < 0.5f)
			{
				float temp = Remap (progress, 0f, 0.5f, 0f, 1f);
				gauge.color = new Color (1f, temp, 0f);

			}

			progress -= increment;
			yield return new WaitForSeconds(smoothness);

			if (stoploop == true)
			{
				break;
			}
		}
		yield break ;
	}


	public IEnumerator O2up()
	{
		print ("up");

		//		float progress = 1; //This float will serve as the 3rd parameter of the lerp function.
		float increment = upMultiplier * smoothness/duration; //The amount of change to apply.
		while(progress <1)
		{

			gauge.fillAmount = progress;
			percent.text = ((progress * 100f).ToString ("##.#") + "%");

			if(progress > 0.5f)
			{
				float temp = Remap (progress, 0.5f, 1f, 0f, 1f);
				gauge.color = new Color (1-temp, 1f, 0f);

			}

			if(progress < 0.5f)
			{
				float temp = Remap (progress, 0f, 0.5f, 0f, 1f);
				gauge.color = new Color (1f, temp, 0f);

			}

			progress += increment;
			yield return new WaitForSeconds(smoothness);
		}
		yield break ;
	}


	public float Remap (float value, float low1, float high1, float low2, float high2)
	{
		return low2 + (value - low1) * (high2 - low2) / (high1 - low1);


	}

	public void StartO2Counter()
	{
		StartCoroutine (O2Down ());
	}

	public void RestoreO2()
	{
		StopCoroutine (O2Down ());
		stoploop = true;
		StartCoroutine (O2up ());
	}

}
