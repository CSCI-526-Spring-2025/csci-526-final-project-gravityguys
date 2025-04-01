using UnityEngine;
using UnityEngine.SceneManagement;

public class DamagePlayerOnHit : MonoBehaviour
{
	public GameObject loseUI;

	void Start()
    {
        if (!loseUI)
        {
            loseUI = GameObject.Find("PlayerPrefab/Canvas/Game Over Screen");
        }
        loseUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag.Equals("Player"))
        {
        	PauseScript.Instance.pausePhysics();
         	loseUI.SetActive(true);

            GameObject respawner = GameObject.FindGameObjectWithTag("Respawn");
            if (respawner)
            {
            	respawner.BroadcastMessage("RespawnPlayer");
             	AnalyticsManager.Instance.IncrementPlayerDeath();
            }
        }
    }
}
