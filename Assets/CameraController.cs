using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // The target player transform
    public CinemachineVirtualCamera virtualCamera;

    [Header("Camera Settings")]
    public float followDistance = 5f; // Distance behind the player
    public float followHeight = 2f;   // Height above the player
    public float rotationSpeed = 5f;  // Speed of rotation

    private CinemachineTransposer transposer;
    private CinemachineComposer composer;

    void Start()
    {
        if (virtualCamera != null)
        {
            transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
            UpdateCameraSettings();
        }
    }

    void Update()
    {
        if (virtualCamera != null)
        {
            HandleCameraRotation();
        }
    }

    private void UpdateCameraSettings()
    {
        if (transposer != null)
        {
            transposer.m_FollowOffset = new Vector3(0, followHeight, -followDistance);
        }

        if (composer != null)
        {
            composer.m_TrackedObjectOffset = new Vector3(0, followHeight, 0);
        }
    }

    private void HandleCameraRotation()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");

        player.Rotate(0, horizontal * rotationSpeed, 0);

        if (composer != null)
        {
            composer.m_TrackedObjectOffset.y += vertical * Time.deltaTime * rotationSpeed;
            composer.m_TrackedObjectOffset.y = Mathf.Clamp(composer.m_TrackedObjectOffset.y, -1, 3);
        }
    }
}
