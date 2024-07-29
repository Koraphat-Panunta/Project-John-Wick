using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    bool aimDownSight = false;
    [SerializeField] public CinemachineFreeLook CinemachineFreeLook;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;    
    }

    void Update()
    {
        //if(aimDownSight == true)
        //{
        //    CinemachineFreeLook.m_Lens.FieldOfView = 40;
        //}
        //else if(aimDownSight == false)
        //{
        //    CinemachineFreeLook.m_Lens.FieldOfView = 65;
        //}
     
    }
    public void AimDownSight(InputAction.CallbackContext Value)
    {
        aimDownSight = Value.phase.IsInProgress();
    }


}
