using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlyController : MonoBehaviour
{
    public float movementSpeed = 10.0f;
    public float fastMovementSpeed = 50.0f;
    public float freeLookSensitivity = 3.0f;
    public float zoomSensitivity = 10.0f;
    public float fastZoomSensitivity = 50.0f;

    private bool looking = false;

    void Update()
    {
        var fastMode = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        var movementSpeed = fastMode ? this.fastMovementSpeed : this.movementSpeed;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= transform.forward * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= transform.right * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += transform.right * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += transform.up * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.position -= transform.up * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartLooking();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            StopLooking();
        }

        if (looking)
        {
            float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSensitivity;
            float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * freeLookSensitivity;
            transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }

        float axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0)
        {
            var zoomSensitivity = fastMode ? this.fastZoomSensitivity : this.zoomSensitivity;
            transform.position += transform.forward * axis * zoomSensitivity;
        }
    }

    private void StartLooking()
    {
        looking = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void StopLooking()
    {
        looking = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
