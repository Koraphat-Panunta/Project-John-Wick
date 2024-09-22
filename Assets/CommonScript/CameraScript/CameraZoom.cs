using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : ICameraAction
{
    private CinemachineCameraOffset cameraOffset;
    private CinemachineFreeLook cinemachineFreeLook;
    private float fovZoomOut;
    private float fovZoomIn;
    private float fovZoomPercentage = 16;
    public CameraZoom(CameraController cameraController)
    {
        this.cameraOffset = cameraController.cameraOffset;
        this.cinemachineFreeLook = cameraController.CinemachineFreeLook;
        fovZoomOut = this.cinemachineFreeLook.m_Lens.FieldOfView;
        Debug.Log("fovZoomOut = " + fovZoomOut);
        fovZoomIn = fovZoomOut - ((16 * fovZoomOut) / 100);

    }
    public void Performed()
    {
       
    }

    public void ZoomIn(Weapon weapon)
    {
        if (weapon == null)
        {
            cameraOffset.m_Offset.z = Mathf.Lerp(cameraOffset.m_Offset.z, 1.6f, 10*Time.deltaTime);
        }
        else
        {
            cameraOffset.m_Offset.z = weapon.weapon_StanceManager.AimingWeight*1.6f;
            Debug.Log("AimingWeight = " + weapon.weapon_StanceManager.AimingWeight);
            cinemachineFreeLook.m_Lens.FieldOfView = Mathf.Lerp(fovZoomOut, fovZoomIn, weapon.weapon_StanceManager.AimingWeight);
        }
    }
    
    public void ZoomOut(Weapon weapon)
    {
        if (weapon == null)
        {
            cameraOffset.m_Offset.z = Mathf.Lerp(cameraOffset.m_Offset.z, 0, 10 * Time.deltaTime);
        }
        else
        {
            cameraOffset.m_Offset.z = weapon.weapon_StanceManager.AimingWeight * 1.6f;
            Debug.Log("AimingWeight = " + weapon.weapon_StanceManager.AimingWeight);
            cinemachineFreeLook.m_Lens.FieldOfView = Mathf.Lerp(fovZoomOut, fovZoomIn, weapon.weapon_StanceManager.AimingWeight);
        }
    }
    
    

    
}
