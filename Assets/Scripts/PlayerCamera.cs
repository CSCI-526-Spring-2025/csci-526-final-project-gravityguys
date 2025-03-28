using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerCamera : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform camHolder;

    public float xRotation;
    public float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 initRot = camHolder.eulerAngles;
        xRotation = initRot[0];
        yRotation = initRot[1];
    }

    private void setCamera(Vector3 lookat)
    {
        xRotation = lookat[0];
        yRotation = lookat[1];

        camHolder.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.localRotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void Update()
    {
        if (!PauseScript.IsGamePaused)
        {
            // get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            if (yRotation > 360) yRotation -= 360;
            if (yRotation < -360) yRotation += 360;

            // rotate cam and orientation
            camHolder.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.localRotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
