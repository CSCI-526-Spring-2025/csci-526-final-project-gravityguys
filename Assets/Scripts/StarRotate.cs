using UnityEngine;

public class StarRotate : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 45, 0); // Adjust as needed

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}