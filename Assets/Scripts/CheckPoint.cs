using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour
{
    bool isActive = false;
    [SerializeField] GameObject indicator;
    [SerializeField] GameObject checkptText;
    GameObject respawnPoint;
    [SerializeField] Transform checkPoint;

    public float displayTime = 1f; // How long to show the text
    public float fadeSpeed = 1f; // Speed of fading

    private CanvasGroup canvasGroup;

    private void Start()
    {
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        Debug.Log("Respawn Point: " + respawnPoint.name);
        var hasRespawner = respawnPoint.GetComponent<Respawner>() != null;
        Debug.Log("Has Respawner: " + hasRespawner);

        if (checkptText != null)
        {
            canvasGroup = checkptText.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = checkptText.AddComponent<CanvasGroup>(); // Add if missing
            }
            canvasGroup.alpha = 0; // Hide text initially
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isActive)
            {
                respawnPoint.BroadcastMessage("SetCheckPoint", this);

                if (indicator != null)
                {
                    indicator.SetActive(true);
                    StartCoroutine(FadeOutCheckPoint());
                }
                AnalyticsManager.Instance.RecordCheckpoint(gameObject.name);
                isActive = true;
            }
        }
    }

    private void DisableCheckPoint()
    {
        if (indicator != null)
        {
            indicator.SetActive(false);
        }
        isActive = false;
    }

    private IEnumerator FadeOutCheckPoint()
    {
        checkptText.SetActive(true);
        canvasGroup.alpha = 1; // Show UI
        yield return new WaitForSeconds(displayTime); // Wait for X seconds

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed; // Fade out smoothly
            yield return null;
        }
        checkptText.SetActive(false); // Hide text completely

    }
}
