using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using System.Threading.Tasks;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;

    [SerializeField] private int levelNum = 0;

    private int deathNum = 0;
    private bool completedLevel = false;
    private string lastPlatformTouched = "Unknown";

    private float levelStartTime;

    private string lastCheckpointID = null;
    private float lastCheckpointTime = 0f;

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

        StartLevelTimer(); // Start tracking time when the level begins
    }

    private async Task InitializeAnalytics()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("✅ Unity Analytics Initialized Successfully!");
    }

    private void StartLevelTimer()
    {
        levelStartTime = Time.time; // Store the time when the level starts
        Debug.Log($"⏳ Level {levelNum} started at {levelStartTime} seconds.");
    }

    public void SetLastPlatformTouched(string platformName)
    {
        lastPlatformTouched = platformName;
        //Debug.Log($"🟢 Last Platform Updated: {lastPlatformTouched}");
    }

    public void IncrementPlayerDeath()
    {
        deathNum++;
        Debug.Log($"☠️ Player Died! Total Deaths: {deathNum}");

        LastPlatformTouchedEvent lastPlatformTouchedEvent = new LastPlatformTouchedEvent(levelNum, lastPlatformTouched);
        AnalyticsService.Instance.RecordEvent(lastPlatformTouchedEvent);
        Debug.Log($"📊 Analytics Event Sent: Last Platform Touched Before Death - {lastPlatformTouched}");
    }

    public void PlayerWon()
    {
        completedLevel = true;

        int timeToCompleteLevel = Mathf.RoundToInt(Time.time - levelStartTime);
        Debug.Log($"🎉 Player Won! Level Completed in {timeToCompleteLevel} seconds. Total Deaths: {deathNum}");

        TimeToCompleteLevelEvent timeEvent = new TimeToCompleteLevelEvent(levelNum, timeToCompleteLevel);
        AnalyticsService.Instance.RecordEvent(timeEvent);
        Debug.Log($"📊 Analytics Event Sent: Time To Complete Level - {timeToCompleteLevel} seconds");

        RecordCheckpoint("LEVEL_END");

        SendLevelAnalytics();
    }

    public void RecordCheckpoint(string checkpointID)
    {
        float currentTime = Time.time;
        int timeSinceLast = Mathf.RoundToInt(currentTime - lastCheckpointTime);

        string fromID = lastCheckpointID ?? "START";

        AnalyticsService.Instance.RecordEvent(new CheckpointTimingEvent(
            levelNum,
            fromID,
            checkpointID,
            timeSinceLast
        ));

        Debug.Log($"📊 Analytics Event Sent: From {fromID} -> {checkpointID}, Time: {timeSinceLast}s");

        lastCheckpointID = checkpointID;
        lastCheckpointTime = currentTime;
    }

    public void ResetCheckpointTiming()
    {
        lastCheckpointID = null;
        lastCheckpointTime = Time.time;
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
        ResetCheckpointTiming();
        StartLevelTimer();
    }

    private void OnApplicationQuit()
    {
        if (deathNum > 0)
        {
            SendLevelAnalytics();
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

public class TimeToCompleteLevelEvent : Unity.Services.Analytics.Event
{
    public TimeToCompleteLevelEvent(int levelNum, int timeInSeconds) : base("timeToCompleteLevel")
    {
        SetParameter("zLevelNum", levelNum);
        SetParameter("zTimeInSeconds", timeInSeconds);
    }
}

public class CheckpointTimingEvent : Unity.Services.Analytics.Event
{
    public CheckpointTimingEvent(int levelNum, string fromCheckpoint, string toCheckpoint, int timeSinceLast) : base("checkpointTiming")
    {
        SetParameter("zLevelNum", levelNum);
        SetParameter("zFromCheckpointID", fromCheckpoint);
        SetParameter("zToCheckpointID", toCheckpoint);
        SetParameter("zTimeSinceLast", timeSinceLast);
        SetParameter("zCheckpointID", toCheckpoint); // for consistency
    }
}
