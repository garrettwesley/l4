using Amazon;
using Amazon.MobileAnalytics.MobileAnalyticsManager;
using Amazon.CognitoIdentity;
using UnityEngine;

public class AwsMobileAnalytics : MonoBehaviour
{
    public MobileAnalyticsManager AnalyticsManager
    {
        get;
        private set;
    }

    // ------------------------------------------------------------------------------------- //

    public void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        DontDestroyOnLoad(base.transform.gameObject);
        this.AnalyticsManager =
            MobileAnalyticsManager.GetOrCreateInstance(
                "cfe9ee065ad44153b0eb141aa1cc83c2",
                new CognitoAWSCredentials("us-east-1:e319fcf7-d397-43c5-b51e-ee7bc9624aa2",
                RegionEndpoint.USEast1),
                Amazon.RegionEndpoint.USEast1);

        SendSystemInfoEvent();
    }

    // ------------------------------------------------------------------------------------- //

    private void SendSystemInfoEvent()
    {
        var systemInfoEvent = new AnalyticEvent(AnalyticEventType.SystemInfo, this.AnalyticsManager);
        systemInfoEvent.AddAttribute("DeviceModel", SystemInfo.deviceModel);
        systemInfoEvent.AddAttribute("DeviceName", SystemInfo.deviceName);
        systemInfoEvent.AddAttribute("DeviceType", SystemInfo.deviceType.ToString());
        systemInfoEvent.AddAttribute("DeviceID", SystemInfo.deviceUniqueIdentifier);
        systemInfoEvent.AddMetric("GraphicsMemorySize", SystemInfo.graphicsMemorySize);
        systemInfoEvent.AddAttribute("OS", SystemInfo.operatingSystem);
        systemInfoEvent.AddMetric("ProcessorCount", SystemInfo.processorCount);
        systemInfoEvent.AddMetric("SystemMemorySize", SystemInfo.systemMemorySize);
        systemInfoEvent.Submit();
    }

    // ------------------------------------------------------------------------------------- //

    public void Update()
    { }

    // ------------------------------------------------------------------------------------- //

    // You want this session management code only in one game object
    // that persists through the game life cycles using “DontDestroyOnLoad (transform.gameObject);”
    public void OnApplicationFocus(bool focus)
    {
        if (this.AnalyticsManager != null)
        {
            if (focus)
            {
                this.AnalyticsManager.ResumeSession();
            }
            else
            {
                this.AnalyticsManager.PauseSession();
            }
        }
    }

    // ------------------------------------------------------------------------------------- //
}
