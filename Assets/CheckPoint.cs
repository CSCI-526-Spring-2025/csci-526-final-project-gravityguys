using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    bool isActive = false;
    [SerializeField] GameObject indicator;
    GameObject respawnPoint;
    [SerializeField] Transform checkPoint;

    private void Start()
    {
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isActive)
            {
                respawnPoint.BroadcastMessage("SetCheckPoint", this);

                indicator.SetActive(true);
                isActive = true;
            }
        }
    }

    private void DisableCheckPoint()
    {
        indicator.SetActive(false);
        isActive = false;
    }
}
