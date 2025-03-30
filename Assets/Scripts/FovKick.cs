using System.Collections;
using UnityEngine;

public class FovKick : MonoBehaviour
{
    public float dashFOVmultiplier = .8f;
    public float fovDuration = 0.1f;
    public float fovRecoverySpeed = 4f;

    private float baseFOV;
    private bool isFOVKicking = false;
    private Camera playerCam;
    
    void Start()
    {
        playerCam = GetComponent<Camera>();
        baseFOV = playerCam.fieldOfView;
    }

    public void TriggerDashFOV()
    {
        StartCoroutine(DashFOVKick());
    }

    IEnumerator DashFOVKick()
    {
        isFOVKicking = true;
        playerCam.fieldOfView *= dashFOVmultiplier;

        yield return new WaitForSeconds(fovDuration);

        isFOVKicking = false;
    }

    void Update()
    {
        if (!isFOVKicking)
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, baseFOV, Time.deltaTime * fovRecoverySpeed);
        }
    }
}