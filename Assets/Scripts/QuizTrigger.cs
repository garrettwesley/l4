using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class QuizTrigger : MonoBehaviour
{
    private DateTime nextTriggerTime;

    public TextAsset QuizToLoad;
    public QuizManager QuizManager;

    // ------------------------------------------------------------------------------------- //

    public void OnTriggerEnter(Collider other)
    {
        if (DateTime.Now < this.nextTriggerTime)
        {
            return;
        }
        this.QuizManager.LoadQuiz(this.QuizToLoad);
        this.nextTriggerTime = DateTime.Now + TimeSpan.FromSeconds(30);
    }

    // ------------------------------------------------------------------------------------- //
}

