using UnityEngine;

public class MovingWall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 ogPos;
    void Start()
    {
        ogPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * 5 * Time.deltaTime);
    }

    private void ResetObject()
    {
        transform.position = ogPos;
    }
}
