using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : ICameraAction
{
    private CinemachineCameraOffset cameraOffset;
    private CinemachineCamera cinemachineCamera;
    private float fovZoomOut;
    private float fovZoomIn = 60;
    private float fovZoomPercentage = 16;
    private float distanceZoomIn = 0.85f;
    public CameraZoom(CameraController cameraController)
    {
        this.cameraOffset = cameraController.thirdPersonCinemachineCamera.cameraOffset;
        this.cinemachineCamera = cameraController.cinemachineCamera;
        fovZoomOut = this.cinemachineCamera.Lens.FieldOfView;
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
            cameraOffset.Offset.z = Mathf.Lerp(cameraOffset.Offset.z, distanceZoomIn, 10*Time.deltaTime);
        }
        else
        {
            cameraOffset.Offset.z = aimingWeight* distanceZoomIn;
            cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(fovZoomOut, fovZoomIn, aimingWeight);
        }
    }
    
    public void ZoomOut(Weapon weapon)
    {

        float aimingWeight = weapon.userWeapon.weaponManuverManager.aimingWeight;

        if (weapon == null)
        {
            cameraOffset.Offset.z = Mathf.Lerp(cameraOffset.Offset.z, 0, 10 * Time.deltaTime);
        }
        else
        {
            cameraOffset.Offset.z = aimingWeight * distanceZoomIn;
            cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(fovZoomOut, fovZoomIn, aimingWeight);
        }
    }
    
    

    
}
