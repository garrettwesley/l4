using Amazon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour
{
    private Dictionary<Alert, Text> alerts;

    public InputField InputField;
    public Text InvalidEmailAlert;
    public Text NotRegisteredAlert;
    public Text NotPaidAlert;
    public Button LoginButton;

    enum Alert
    {
        InvalidEmail = 0,
        NotRegistered,
        NotPaid,
        NumAlerts,
    }

    // ------------------------------------------------------------------------------------- //

    public void Awake()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        this.alerts = new Dictionary<Alert, Text>()
        {
            { Alert.InvalidEmail, this.InvalidEmailAlert },
            { Alert.NotRegistered, this.NotRegisteredAlert },
            { Alert.NotPaid, this.NotPaidAlert },
        };
    }

    // ------------------------------------------------------------------------------------- //

    private void EnableAlert(Alert alert)
    {
        for (int i = 0; i < (int)Alert.NumAlerts; i++)
        {
            Alert current = (Alert)i;
            this.alerts[current].gameObject.SetActive(current == alert);
        }
    }

    // ------------------------------------------------------------------------------------- //

    public void OnSubmitPressed()
    {
        EnableAlert(Alert.NumAlerts); // remove previous alerts
        if (string.IsNullOrEmpty(this.InputField.text) ||
            !this.InputField.text.Contains("@"))
        {
            EnableAlert(Alert.InvalidEmail);
            return;
        }
        this.LoginButton.interactable = false;
        GameProgress.LoadUserData(
            this.InputField.text,
            success =>
            {
                this.LoginButton.interactable = true;

                if (!success)
                {
                    EnableAlert(Alert.NotRegistered);
                    return;
                }

                if (GameProgress.UserData.Paid)
                {
                    SceneManager.LoadScene("IntroMarsOrbit");
                    return;
                }

                EnableAlert(Alert.NotPaid);
            });
    }

    // ------------------------------------------------------------------------------------- //

	public void Exit ()
	{
		Application.Quit ();
	}

}
