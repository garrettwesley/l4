using Amazon.MobileAnalytics.MobileAnalyticsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum AnalyticEventType
{
    PlayerLocation,
    SystemInfo,
    QuizResponse,
}

public class AnalyticEvent
{
    private string name;
    private MobileAnalyticsManager analyticsManager;
    private Dictionary<string, double> metrics;
    private Dictionary<string, string> attributes;

    // ------------------------------------------------------------------------------------- //

    public AnalyticEvent(AnalyticEventType analyticsEventType, MobileAnalyticsManager analyticsManager)
    {
        this.name = analyticsEventType.ToString();
        this.analyticsManager = analyticsManager;
        this.metrics = new Dictionary<string, double>();
        this.attributes = new Dictionary<string, string>();
    }

    // ------------------------------------------------------------------------------------- //

    public void AddMetric(string name, double value)
    {
        this.metrics[name] = value;
    }

    // ------------------------------------------------------------------------------------- //

    public void AddAttribute(string name, string value)
    {
        this.attributes[name] = value;
    }

    // ------------------------------------------------------------------------------------- //

    public void Submit()
    {
        CustomEvent customEvent = new CustomEvent(this.name);
        foreach (KeyValuePair<string, string> kvp in this.attributes)
        {
            customEvent.AddAttribute(kvp.Key, kvp.Value);
        }
        foreach (KeyValuePair<string, double> kvp in this.metrics)
        {
            customEvent.AddMetric(kvp.Key, kvp.Value);
        }
        this.analyticsManager.RecordEvent(customEvent);
    }

    // ------------------------------------------------------------------------------------- //
}
