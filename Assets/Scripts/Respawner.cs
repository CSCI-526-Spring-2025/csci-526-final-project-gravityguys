using UnityEngine;

public class Respawner : MonoBehaviour
{
    public GameObject Player;
    public GameObject Respawn;

    private void RespawnPlayer()
    {
        Player.transform.position = Respawn.transform.position;
        Player.transform.rotation = Respawn.transform.rotation;
    }
}
