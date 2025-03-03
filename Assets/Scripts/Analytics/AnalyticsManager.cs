using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using System.Threading.Tasks;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;
    private int deathCount = 0;

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

    // Track player death
    public void TrackPlayerDeath()
    {
        deathCount++;
        Debug.Log($"☠️ Player Died! Total Deaths: {deathCount}");
    }

    // Track player reaching the goal
    public void TrackWin()
    {
        PlayerWinEvent winEvent = new PlayerWinEvent(deathCount);
        AnalyticsService.Instance.RecordEvent(winEvent);
        Debug.Log($"🎉 Player Won! Total Deaths: {deathCount} | Event Sent to Analytics");
    }
}

public class PlayerWinEvent : Unity.Services.Analytics.Event
{
    public PlayerWinEvent(int totalDeaths) : base("playerDeathsUntilVictoryLevel1")
    {
        SetParameter("totalDeaths", totalDeaths);
    }
}
