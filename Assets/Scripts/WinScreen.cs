using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public GameObject winUI;

    void Start()
    {
        if (!winUI)
        {
            winUI = GameObject.Find("PlayerPrefab/Canvas/Win Screen");
        }
        winUI.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            AnalyticsManager.Instance.PlayerWon();
            PauseScript.Instance.pausePhysics();
            winUI.SetActive(true);
            if (SceneManager.GetActiveScene().name == "LevelThreeScene")
            {
                GameObject nextLevelButton = GameObject.Find("Next Level Button");
                if(nextLevelButton)
                    nextLevelButton.SetActive(false);
            }
        }
    }
}
