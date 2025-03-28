using UnityEngine;

public class ChangeGravity : MonoBehaviour
{
    public bool checkForEnter = true;
    private void OnTriggerEnter(Collider other)
    {
        if (checkForEnter)
        {
            if (other.CompareTag("Player"))
            {
                if (other.transform.parent.gameObject.GetComponent<PlayerMovement>().state == PlayerMovement.MovementState.air)
                    other.transform.parent.BroadcastMessage("ShiftGravity", this);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (checkForEnter)
        {
            if (other.CompareTag("Player"))
            {
                if (other.transform.parent.gameObject.GetComponent<PlayerMovement>().state == PlayerMovement.MovementState.air)
                    if (Vector3.Dot(this.transform.up, other.transform.up) < 0.95)
                        other.transform.parent.BroadcastMessage("ShiftGravity", this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.parent.gameObject.GetComponent<GravityController>().activeGravitySource == this)
            {
                {
                    //if (other.transform.parent.gameObject.GetComponent<PlayerMovement>().state == PlayerMovement.MovementState.air)
                    {
                        other.transform.parent.BroadcastMessage("ShiftGravity", Vector3.down);
                    }
                }
            }
        }
    }
    
}
