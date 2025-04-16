using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    Vector3 pos;
    Quaternion rot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pos = this.transform.position;
        rot = this.transform.rotation;
    }

    private void ResetObject()
    {
        this.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        this.transform.position = pos;
        this.transform.rotation = rot;
    }
}
