using UnityEngine;
using System.Collections;

public class FadeWin : MonoBehaviour
{
    public GameObject winTextUI; // Assign this in the Inspector
    public float displayTime = 2f; // How long to show the text
    public float fadeSpeed = 1f; // Speed of fading

    private CanvasGroup canvasGroup;

    void Start()
    {
        if (winTextUI != null)
        {
            canvasGroup = winTextUI.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = winTextUI.AddComponent<CanvasGroup>(); // Add if missing
            }
            canvasGroup.alpha = 0; // Hide text initially
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            //Respawn when player reaches goal
            GameObject respawner = GameObject.FindGameObjectWithTag("Respawn");
            if (respawner)
                respawner.BroadcastMessage("RespawnPlayer");
            

            //Make the goal fade out
            StartCoroutine(FadeOutWin());


        }
    }

    private IEnumerator FadeOutWin()
    {
        winTextUI.SetActive(true);
        canvasGroup.alpha = 1; // Show UI
        yield return new WaitForSeconds(displayTime); // Wait for X seconds

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed; // Fade out smoothly
            yield return null;
        }

        winTextUI.SetActive(false); // Hide text completely

    }
}
