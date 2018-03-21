using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerLocationTracker : MonoBehaviour
{
    public float CadenceInSeconds = 15;
    public AwsMobileAnalytics AwsMobileAnalytics;
    public GameObject Player;

    // ------------------------------------------------------------------------------------- //

    public void Start()
    {
        StartCoroutine(LogPlayerPosition());
    }

    // ------------------------------------------------------------------------------------- //

    public void Update()
    { }

    // ------------------------------------------------------------------------------------- //

    private IEnumerator LogPlayerPosition()
    {
        yield return new WaitForSeconds(this.CadenceInSeconds);
        if (this.Player != null && this.AwsMobileAnalytics != null)
        {
            Vector3 position = this.Player.gameObject.transform.position;
            var evt = new AnalyticEvent(AnalyticEventType.PlayerLocation, this.AwsMobileAnalytics.AnalyticsManager);
            evt.AddMetric("x", position[0]);
            evt.AddMetric("y", position[1]);
            evt.AddMetric("z", position[2]);
            evt.Submit();
            Debug.Log(string.Format("Submitted player location: {0}", position));
        }
        yield return LogPlayerPosition();
    }

    // ------------------------------------------------------------------------------------- //
}
