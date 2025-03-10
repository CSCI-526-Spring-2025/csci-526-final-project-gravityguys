using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public GameObject winUI;

    void Start()
    {
        winUI.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            AnalyticsManager.Instance.PlayerWon();
            PauseScript.Instance.pausePhysics();
            winUI.SetActive(true);
        }
    }
}
