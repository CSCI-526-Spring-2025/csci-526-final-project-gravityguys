using UnityEngine;

public class DamagePlayerOnHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag.Equals("Player"))
        {
            GameObject respawner = GameObject.FindGameObjectWithTag("Respawn");
            if (respawner)
                respawner.BroadcastMessage("RespawnPlayer");
                AnalyticsManager.Instance.IncrementPlayerDeath();
        }
    }
}
