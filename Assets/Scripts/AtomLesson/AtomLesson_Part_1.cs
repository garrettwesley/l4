using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;





public class AtomLesson_Part_1 : MonoBehaviour {


    private Quiz quiz;
    private Vector3 originalCameraPosition;

    public Font Font;
    public GameObject AtomPreFab;
    public FirstPersonController FirstPersonController;
 
    public Button yourButton;
    public Text Text1;
    public Button yourButton2;
    public Text Text2;
    public Button yourButton3;
    public Text Text3;
    public Button yourButton4;

    public void LoadCamera(TextAsset quizToLoad)
    {
        this.quiz = JsonUtility.FromJson<Quiz>(quizToLoad.text);

        if (this.FirstPersonController != null &&
            this.quiz.CameraLocationX != 0 &&
            this.quiz.CameraLocationY != 0 &&
            this.quiz.CameraLocationZ != 0)
        {
            this.originalCameraPosition = this.FirstPersonController.transform.position;
            Debug.Log(string.Format("Setting fps position to {0}", new Vector3(this.quiz.CameraLocationX, this.quiz.CameraLocationY, this.quiz.CameraLocationZ)));
            this.FirstPersonController.transform.position = new Vector3(
                this.quiz.CameraLocationX, 
                this.quiz.CameraLocationY, 
                this.quiz.CameraLocationZ);
            Camera c = this.FirstPersonController.GetComponentInChildren<Camera>();
            c.gameObject.transform.LookAt(
                new Vector3(
                    this.quiz.CameraLookAtX,
                    this.quiz.CameraLookAtY,
                    this.quiz.CameraLookAtZ));
            this.FirstPersonController.GetComponent<FirstPersonController>().enabled = false;
      
        }
    }
    public void onClick()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick_1);

        Button btn2 = yourButton2.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick_2);

        Button btn3 = yourButton3.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick_3);

        Button btn4 = yourButton4.GetComponent<Button>();
        btn.onClick.AddListener(ExitButtonClick);
    }

   public void TaskOnClick_1()
    {
        Debug.Log("You have clicked the #1 button!");
        Text1.GetComponent<Text>().enabled = true;
        Text2.GetComponent<Text>().enabled = false;
        Text3.GetComponent<Text>().enabled = false;
    //    AtomPreFab.SetActive(true);

    }

    public void TaskOnClick_2()
    {
        Debug.Log("You have clicked the #2 button!");
        Text1.GetComponent<Text>().enabled = false;
        Text2.GetComponent<Text>().enabled = true;
        Text3.GetComponent<Text>().enabled = false;
    }

   public void TaskOnClick_3()
    {
        Debug.Log("You have clicked the #3 button!");
        Text1.GetComponent<Text>().enabled = false;
        Text2.GetComponent<Text>().enabled = false;
        Text3.GetComponent<Text>().enabled = true;
    }

    public void ExitButtonClick()
    {
        if (this.FirstPersonController != null &&
           this.quiz.CameraLocationX != 0 &&
           this.quiz.CameraLocationY != 0 &&
           this.quiz.CameraLocationZ != 0)
        {
            Debug.Log("Resetting fps controller");
            this.FirstPersonController.transform.position = this.originalCameraPosition;
         //  this.FirstPersonController.GetComponentInChildren<Camera>().cullingMask = this.orignalCullingMask;
            this.FirstPersonController.GetComponent<FirstPersonController>().enabled = true;
            Text1.GetComponent<Text>().enabled = false;
        //    Text2.GetComponent<Text>().enabled = false;
         //   Text3.GetComponent<Text>().enabled = false;
        }
    }

}


