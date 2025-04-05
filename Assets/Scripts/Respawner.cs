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
        if (!Player)
        {
            Player = FindAnyObjectByType<PlayerMovement>().gameObject;
        }
    }

    private void RespawnPlayer()
    {
        Player.transform.position = Respawn.transform.position;
        GravityController gc = Player.GetComponent<GravityController>();
        if (gc)
        {
            gc.BroadcastMessage("ShiftGravity", Vector3.down);
        }
        Player.transform.rotation = Quaternion.Euler(0, 0, 0);
        Player.transform.parent.GetChild(2).transform.rotation = Quaternion.Euler(0, 0, 0);
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
