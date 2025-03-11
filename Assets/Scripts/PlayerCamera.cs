using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform camHolder;
    private PlayerInput playerInput; 

    float xRotation;
    float yRotation;
    

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        Vector2 lookVector = playerInput.actions["Look"].ReadValue<Vector2>();
        // get mouse input
        float mouseX = lookVector.x;//Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensX;
        float mouseY = lookVector.y;//Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensY;
        Debug.Log("MouseX: " + mouseX +", MouseY: " + mouseY);
        if (!PauseScript.IsGamePaused)
        {
            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // rotate cam and orientation
            camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
