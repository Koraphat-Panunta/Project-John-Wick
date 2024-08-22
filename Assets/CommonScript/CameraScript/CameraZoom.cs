using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : ICameraAction
{
    private CinemachineCameraOffset cameraOffset;
    public CameraZoom(CameraController cameraController)
    {
        this.cameraOffset = cameraController.cameraOffset;
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
            cameraOffset.m_Offset.z = weapon.weapon_StanceManager.AimingWeight;
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
            cameraOffset.m_Offset.z = weapon.weapon_StanceManager.AimingWeight;
        }
    }
    
    

    
}
