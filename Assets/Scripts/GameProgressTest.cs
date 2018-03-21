using Amazon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressTest : MonoBehaviour
{
    // ------------------------------------------------------------------------------------- //

    public void Awake()
    {
        //PlayerPrefs.DeleteAll();
        UnityInitializer.AttachToGameObject(this.gameObject);
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("email")))
        {
            PlayerPrefs.SetString("email", "nanolumina@gmail.com");
        }
        GameProgress.LoadUserData(PlayerPrefs.GetString("email"));
        StartCoroutine(LoadUserData());
    }

    private IEnumerator LoadUserData()
    {
        yield return new WaitForSeconds(5);
        GameProgress.MarkCheckpointComplete(Checkpoint.Lesson1Complete);
    }

    // ------------------------------------------------------------------------------------- //

    public void Update()
    { }

    // ------------------------------------------------------------------------------------- //
}
