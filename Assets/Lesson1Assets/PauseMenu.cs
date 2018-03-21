using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;


public class PauseMenu : MonoBehaviour {

	private bool isGamePaused = false;
	private bool controllerActive;
	private Scene currentScene;

	public FirstPersonController fpc;
	public GameObject pausePanel;
	public GameObject controls;
	public GameObject areYouSure;
	public GameObject areYouSure_mainMenu;
	public GameObject badge;
	public GameObject loadLevelPanel;
	public GameObject debuggingButton;
	public bool pauseMenuAccessible = true;

	MasterControlScript mcs;


	void Start () {

		mcs = GameObject.Find ("DontDestroyonLoad").GetComponent<MasterControlScript> ();
		currentScene = SceneManager.GetActiveScene ();


	}
	
	public void Update () {

		if (controllerActive == true)
		{
			Cursor.visible = true;	
			Cursor.lockState = CursorLockMode.None;
		}

		if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) && pauseMenuAccessible == true)
		{
			if (isGamePaused == false)
			{
				StartCoroutine (OpenMenu ());

			}

			if (isGamePaused == true)
			{
				StartCoroutine (CloseMenu ());

			}
	
		}

		if (isGamePaused == true && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.U))
		{
			debuggingButton.SetActive (true);
		}

	}

	public IEnumerator OpenMenu()
	{
		controllerActive = true;
		fpc.enabled = false;
		pausePanel.SetActive (true);
		yield return new WaitForSeconds (0.1f);
		isGamePaused = true;
		print (currentScene.name);
		if(mcs.badgePickedUp == true && currentScene.name == "spaceship_master2")
		{
			badge.SetActive (true);
		}

	}

	public IEnumerator CloseMenu()
	{
		controllerActive = false;
		fpc.enabled = true;
		pausePanel.SetActive (false);
		yield return new WaitForSeconds (0.1f);
		isGamePaused = false;
		areYouSure.SetActive (false);
		controls.SetActive (false);



	}

	public void AreYouSure()
	{
		areYouSure.SetActive (true);

	}

	public void Cancel()
	{
		areYouSure.SetActive (false);

	}


	public void QuitGame()
	{
		Application.Quit() ;

	}

	public void OpenControls()
	{
		controls.SetActive (true);
	}

	public void CloseControls()
	{
		controls.SetActive (false);
	}

	public void ResumeGame()
	{
		fpc.enabled = true;
		pausePanel.SetActive (false);
		isGamePaused = false;


	}

	public void LoadAreYouSureMainMenu()
	{
		areYouSure_mainMenu.SetActive (true);
	}

	public void CloseAreYouSureMainMenu()
	{
		areYouSure_mainMenu.SetActive (false);
	}



	public void ShowLevelLoader()
	{
		loadLevelPanel.SetActive (true);
	}

	public void CancelLevelLoader() 
	{
		loadLevelPanel.SetActive (false);
	}

	public void LoadMainMenu()
	{
		SceneManager.LoadScene ("IntroMarsOrbit");
		mcs.gameCompleted = false;
		mcs.finalShipSequence = false;
		mcs.holoDeckLoaded = false;
		mcs.bridgeDoorUnlocked = false;
		mcs.crewDoorsUnlocked = false;
		mcs.labDoorsUnlocked = false;
		mcs.engineDoorUnlocked = false;
		mcs.badgePickedUp = false;
		mcs.wokeUp = false;
		mcs.firstTimeInShip = true;
		mcs.backInDaShip = false;
		mcs.reloadRedAlert = false;
		mcs.finalShipSequence = false;
		mcs.exitSequece = false;


	}

	public void LoadShipBridge()
	{
		SceneManager.LoadScene ("spaceship_master2");
		mcs.gameCompleted = false;
		mcs.finalShipSequence = false;
		mcs.holoDeckLoaded = false;
		mcs.bridgeDoorUnlocked = false;
		mcs.crewDoorsUnlocked = false;
		mcs.labDoorsUnlocked = false;
		mcs.engineDoorUnlocked = false;
		mcs.badgePickedUp = false;
		mcs.wokeUp = false;
		mcs.firstTimeInShip = true;
		mcs.backInDaShip = false;
		mcs.reloadRedAlert = false;
		mcs.finalShipSequence = false;
		mcs.exitSequece = false;


	}

	public void LoadHolodeck()
	{

		SceneManager.LoadScene ("Holodeck2");
		mcs.gameCompleted = false;
		mcs.finalShipSequence = false;
		mcs.holoDeckLoaded = true;
		mcs.bridgeDoorUnlocked = true;
		mcs.crewDoorsUnlocked = true;
		mcs.labDoorsUnlocked = true;
		mcs.engineDoorUnlocked = false;
		mcs.badgePickedUp = true;
		mcs.wokeUp = false;
		mcs.firstTimeInShip = false;
		mcs.backInDaShip = false;
		mcs.reloadRedAlert = false;
		mcs.finalShipSequence = false;
		mcs.exitSequece = false;

	}

	public void LoadBackInDaShip()
	{
		SceneManager.LoadScene ("spaceship_master2");
		mcs.inHolodeck = true;
		mcs.gameCompleted = false;
		mcs.finalShipSequence = false;
		mcs.holoDeckLoaded = true;
		mcs.bridgeDoorUnlocked = true;
		mcs.crewDoorsUnlocked = true;
		mcs.labDoorsUnlocked = true;
		mcs.engineDoorUnlocked = false;
		mcs.badgePickedUp = true;
		mcs.wokeUp = false;
		mcs.firstTimeInShip = false;
		mcs.backInDaShip = true;
		mcs.reloadRedAlert = false;
		mcs.finalShipSequence = false;
		mcs.exitSequece = false;

	}

	public void LoadRedAlertSequence ()
	{
	
		SceneManager.LoadScene ("spaceship_master2");
		mcs.inCrew = true;
		mcs.gameCompleted = false;
		mcs.finalShipSequence = false;
		mcs.holoDeckLoaded = true;
		mcs.bridgeDoorUnlocked = true;
		mcs.crewDoorsUnlocked = true;
		mcs.labDoorsUnlocked = true;
		mcs.engineDoorUnlocked = false;
		mcs.badgePickedUp = true;
		mcs.wokeUp = true;
		mcs.firstTimeInShip = false;
		mcs.backInDaShip = false;
		mcs.reloadRedAlert = true;
		mcs.finalShipSequence = false;
		mcs.exitSequece = false;

	}

	public void LoadFinalSequence ()
	{

		SceneManager.LoadScene ("IntroMarsOrbit");
		mcs.gameCompleted = false;
		mcs.finalShipSequence = true;
		mcs.holoDeckLoaded = true;
		mcs.bridgeDoorUnlocked = true;
		mcs.crewDoorsUnlocked = true;
		mcs.labDoorsUnlocked = true;
		mcs.engineDoorUnlocked = false;
		mcs.badgePickedUp = true;
		mcs.wokeUp = true;
		mcs.firstTimeInShip = false;
		mcs.backInDaShip = false;
		mcs.reloadRedAlert = false;
		mcs.finalShipSequence = true;
		mcs.exitSequece = true;

	}


}
