using UnityEngine;

public class ResetFallenObjects : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Pullable"))
        {
            other.gameObject.BroadcastMessage("ResetObject");
        }
    }
}
