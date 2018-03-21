using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;





public class holodeck_controller : MonoBehaviour {


	public Button[] yourButton;
	public GameObject lesson;
	public Image lessonImage;
	public Sprite[] sprites;
	public int QuizObjectsLayer = 9;
	public Canvas Canvas;
	public FirstPersonController FPC;
	public Camera FPC_camera;
	public Camera HoloCam;
	public Animator anim;

	ObjectivesUI objUI;
	MasterControlScript MCS;

	private GameObject ddol;
	private bool[] buttonSelected = new bool[5];
	private Vector3 originalCameraPosition;
	private bool controllerActive;
	private PauseMenu pauseMenu;






	void Start () 
	{
		pauseMenu = GameObject.Find ("PauseMenu").GetComponent<PauseMenu> ();
		HoloCam.enabled = false;
		GameObject ddol = GameObject.Find("DontDestroyonLoad");
		MCS = ddol.gameObject.GetComponent<MasterControlScript>();
		lessonImage = lesson.gameObject.GetComponent<Image> ();

		objUI = GameObject.Find ("ObjectivesUI").GetComponent<ObjectivesUI> ();

		Time.timeScale = 1;

	}
	
	void Update () 
	{

		if (controllerActive == true)
		{
			Cursor.visible = true;	
			Cursor.lockState = CursorLockMode.None;
		}

	}

	void OnTriggerEnter(Collider other)
	{

		if (other.tag == "Player") {
//			StartCoroutine (panelCameraZoom ());
			LoadInterface();

		}
		print ("test");
	}
	void LoadInterface ()
	{
		anim.SetBool ("cameraAnim", true);
		pauseMenu.pauseMenuAccessible = false;
		controllerActive = true;
		HoloCam.enabled = true;
		FPC_camera.enabled = false;
		this.FPC.GetComponent<FirstPersonController>().enabled = false;
		objUI.ActivateHUD (false);
		onClick ();

	}
		

	IEnumerator panelCameraZoom ()
	{
		anim.SetBool ("cameraAnim", true);
		yield return new WaitForSeconds(0.5f);
		HoloCam.enabled = true;
		FPC_camera.enabled = false;
		this.FPC.GetComponent<FirstPersonController>().enabled = false;
		Cursor.visible = true;

	}

	public void onClick()
	{

		Button btn0 = yourButton[0].GetComponent<Button>();
//		btn0.onClick.AddListener(TaskOnClick_0);

		Button btn1 = yourButton[1].GetComponent<Button>();
//		btn1.onClick.AddListener(TaskOnClick_1);

		Button btn2 = yourButton[2].GetComponent<Button>();
//		btn2.onClick.AddListener(TaskOnClick_2);

		Button btn3 = yourButton[3].GetComponent<Button>();
//		btn3.onClick.AddListener(TaskOnClick_3);

//		Button btn4 = yourButton[4].GetComponent<Button>();
//		btn4.onClick.AddListener(TaskOnClick_4);

		Button btn5 = yourButton[5].GetComponent<Button>();
//		btn5.onClick.AddListener(TaskOnClick_5);
	
		Button btn6 = yourButton[6].GetComponent<Button>();
//		btn6.onClick.AddListener(ExitButtonClick);
	}


	public void TaskOnClick_0()
	{
		Debug.Log("You have clicked the #1 button!");

		lessonImage.sprite = sprites [0];
		buttonSelected [0] = true;
		buttonSelected [1] = false;
		buttonSelected [2] = false;
		buttonSelected [3] = false;
		buttonSelected [4] = false;

		}

	public void TaskOnClick_1()
	{
		lessonImage.sprite = sprites [1];
		buttonSelected [0] = false;
		buttonSelected [1] = true;
		buttonSelected [2] = false;
		buttonSelected [3] = false;
		buttonSelected [4] = false;
	}

	public void TaskOnClick_2()

	{
		lessonImage.sprite = sprites [1];
		buttonSelected [0] = false;
		buttonSelected [1] = false;
		buttonSelected [2] = true;
		buttonSelected [3] = false;
		buttonSelected [4] = false;
	}

	public void TaskOnClick_3()
	{
		lessonImage.sprite = sprites [1];
		buttonSelected [0] = false;
		buttonSelected [1] = false;
		buttonSelected [2] = false;
		buttonSelected [3] = true;
		buttonSelected [4] = false;
	}

	public void TaskOnClick_4()
	{
		lessonImage.sprite = sprites [1];
		buttonSelected [0] = false;
		buttonSelected [1] = false;
		buttonSelected [2] = false;
		buttonSelected [3] = false;
		buttonSelected [4] = true;
	}

	public void TaskOnClick_5()
	{
		print ("testss");
		if (buttonSelected[0] == true)
		{
			MCS.holoDeckLoaded = true; 
			SceneManager.LoadScene ("Holodeck2");
		}
	}



	public void ExitButtonClick()
	{
		anim.SetBool ("cameraAnim", false);
		controllerActive = false;
		HoloCam.enabled = false;
		FPC_camera.enabled = true;
		this.FPC.GetComponent<FirstPersonController>().enabled = true;
		pauseMenu.pauseMenuAccessible = true;



	}


}

