using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using System.Threading.Tasks;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;

    [SerializeField] private int levelNum = 1;

    private int deathNum = 0;
    private bool completedLevel = false;

    private string lastPlatformTouched = "Unknown";

    async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            await InitializeAnalytics();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async Task InitializeAnalytics()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("✅ Unity Analytics Initialized Successfully!");
    }

    public void SetLastPlatformTouched(string platformName)
    {
        lastPlatformTouched = platformName;
        Debug.Log($"🟢 Last Platform Updated: {lastPlatformTouched}");
    }

    public void IncrementPlayerDeath()
    {
        deathNum++;
        Debug.Log($"☠️ Player Died! Total Deaths: {deathNum}");

        // Send analytics event for last touched platform before death
        LastPlatformTouchedEvent lastPlatformTouchedEvent = new LastPlatformTouchedEvent(levelNum, lastPlatformTouched);
        AnalyticsService.Instance.RecordEvent(lastPlatformTouchedEvent);
        Debug.Log($"📊 Analytics Event Sent: Last Platform Touched Before Death - {lastPlatformTouched}");
    }

    // Track when the player reaches the goal (completing the level)
    public void PlayerWon()
    {
        completedLevel = true;
        Debug.Log($"🎉 Player Won! Total Deaths: {deathNum}");
        SendLevelAnalytics();
    }

    // Track and send data when the player quits or moves to the next level
    private void SendLevelAnalytics()
    {
        DeathCounterEvent deathCounterEvent = new DeathCounterEvent(completedLevel, deathNum, levelNum);
        AnalyticsService.Instance.RecordEvent(deathCounterEvent);

        Debug.Log($"📊 Analytics Event Sent: completedLevel={completedLevel}, deathNum={deathNum}, levelNum={levelNum}");

        // Reset for the next level
        deathNum = 0;
        completedLevel = false;
    }

    private void OnApplicationQuit()
    {
        if (deathNum > 0)
        {
            SendLevelAnalytics(); // Ensure data is sent when the game is closed
        }
    }
}

public class DeathCounterEvent : Unity.Services.Analytics.Event
{
    public DeathCounterEvent(bool completedLevel, int deathNum, int levelNum) : base("deathCounter")
    {
        SetParameter("zCompletedLevel", completedLevel);
        SetParameter("zDeathNum", deathNum);
        SetParameter("zLevelNum", levelNum);
    }
}

public class LastPlatformTouchedEvent : Unity.Services.Analytics.Event
{
    public LastPlatformTouchedEvent(int levelNum, string platformName) : base("lastPlatformTouched")
    {
    	SetParameter("zLevelNum", levelNum);
        SetParameter("zPlatformName", platformName);
    }
}
