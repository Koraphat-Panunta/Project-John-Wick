using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static CamerOverShoulder;

public class CameraController : MonoBehaviour,IObserverPlayer
{
    [SerializeField] public CinemachineFreeLook CinemachineFreeLook;
    [SerializeField] public PlayerController playerController;
    [SerializeField] public CinemachineCameraOffset cameraOffset;
    private ICameraAction cameraOverShoulder;
    
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;    
        playerController.AddObserver(this);
        cameraOverShoulder = new CamerOverShoulder(this);
    }

    void Update()
    {
    }
   

    public void OnNotify(PlayerController playerController,PlayerController.PlayerAction playerAction)
    {
        if (playerAction == SubjectPlayer.PlayerAction.SwapShoulder)
        {
            cameraOverShoulder.Performed();
        }
    }
}
