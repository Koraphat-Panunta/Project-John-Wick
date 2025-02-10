using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : ICameraAction
{
    private CinemachineCameraOffset cameraOffset;
    private CinemachineFreeLook cinemachineFreeLook;
    private float fovZoomOut;
    private float fovZoomIn = 60;
    private float fovZoomPercentage = 16;
    private float distanceZoomIn = 0.85f;
    public CameraZoom(CameraController cameraController)
    {
        this.cameraOffset = cameraController.cameraOffset;
        this.cinemachineFreeLook = cameraController.CinemachineFreeLook;
        fovZoomOut = this.cinemachineFreeLook.m_Lens.FieldOfView;
        //fovZoomIn = fovZoomOut - ((fovZoomPercentage * fovZoomOut) / 100);

    }
    public void Performed()
    {
       
    }

    public void ZoomIn(Weapon weapon)
    {
        float aimingWeight = weapon.userWeapon.weaponManuverManager.aimingWeight;
        if (weapon == null)
        {
            cameraOffset.m_Offset.z = Mathf.Lerp(cameraOffset.m_Offset.z, distanceZoomIn, 10*Time.deltaTime);
        }
        else
        {
            cameraOffset.m_Offset.z = aimingWeight* distanceZoomIn;
            cinemachineFreeLook.m_Lens.FieldOfView = Mathf.Lerp(fovZoomOut, fovZoomIn, aimingWeight);
        }
    }
    
    public void ZoomOut(Weapon weapon)
    {

        float aimingWeight = weapon.userWeapon.weaponManuverManager.aimingWeight;

        if (weapon == null)
        {
            cameraOffset.m_Offset.z = Mathf.Lerp(cameraOffset.m_Offset.z, 0, 10 * Time.deltaTime);
        }
        else
        {
            cameraOffset.m_Offset.z = aimingWeight * distanceZoomIn;
            cinemachineFreeLook.m_Lens.FieldOfView = Mathf.Lerp(fovZoomOut, fovZoomIn, aimingWeight);
        }
    }
    
    

    
}
