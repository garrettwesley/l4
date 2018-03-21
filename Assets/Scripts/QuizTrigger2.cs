using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class QuizTrigger2 : MonoBehaviour
{
    private DateTime nextTriggerTime;

    public TextAsset QuizToLoad;
    public SmartHUD CamLoad;

    // ------------------------------------------------------------------------------------- //

    public void OnTriggerEnter(Collider other)
    {
        if (DateTime.Now < this.nextTriggerTime)
        {
            return;
        }
        this.CamLoad.LoadCamera(this.QuizToLoad);
     this.nextTriggerTime = DateTime.Now + TimeSpan.FromSeconds(10);
    }

    // ------------------------------------------------------------------------------------- //
}

