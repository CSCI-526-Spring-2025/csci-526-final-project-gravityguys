using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveLaser : MonoBehaviour
{
    [SerializeField] List<Transform> LaserTransforms;
    [SerializeField] GameObject Laser;
    [SerializeField] float TimeToNext;
    [SerializeField] float TimeInBetween;
    float currentTime;
    bool isMoving;

    [SerializeField] int index = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMoving = true;
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (!isMoving)
        {
            if (currentTime >= TimeInBetween)
            {
                currentTime -= TimeInBetween;
                isMoving = true;
            }
        }
        if (isMoving)
        {
            if (currentTime > TimeToNext)
            {
                currentTime -= TimeToNext;
                index = (index + 1) % LaserTransforms.Count;
                isMoving = false;

                Laser.transform.localPosition = LaserTransforms[index].transform.localPosition;
                Laser.transform.localRotation = LaserTransforms[index].transform.localRotation;
            }
            else
            {
                float u = currentTime / TimeToNext;
                int nextIndex = (index + 1) % LaserTransforms.Count;
                Vector3 translation = Vector3.Lerp(LaserTransforms[index].transform.localPosition, LaserTransforms[nextIndex].transform.localPosition, u);
                Quaternion rotation = Quaternion.Slerp(LaserTransforms[index].transform.localRotation, LaserTransforms[nextIndex].transform.localRotation, u);

                Laser.transform.localPosition = translation;
                Laser.transform.localRotation = rotation;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }
}
