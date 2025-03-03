using Unity.Services.Analytics;
using Unity.Services.Core;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("✅ Unity Analytics Initialized Successfully!");
    }
}
