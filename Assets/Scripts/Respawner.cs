using UnityEngine;

public class Respawner : MonoBehaviour
{
    public GameObject Player;
    public GameObject Respawn;

    private CheckPoint mark;

    private Vector3 originalLocation;
    private void Start()
    {
        originalLocation = this.transform.position;
    }

    private void RespawnPlayer()
    {
        Player.transform.position = Respawn.transform.position;
        //Player.transform.rotation = Respawn.transform.rotation;
    }

    private void SetCheckPoint(CheckPoint newCheckPoint)
    {
        if(mark)
            mark.BroadcastMessage("DisableCheckPoint");
        mark = newCheckPoint;
        Respawn.transform.position = mark.transform.position;
        //Respawn.transform.rotation = mark.transform.rotation;
        Debug.Log("Set CheckPoint");
    }

    private void ResetSpawnLocation()
    {
        this.transform.position = originalLocation;
    }
}
