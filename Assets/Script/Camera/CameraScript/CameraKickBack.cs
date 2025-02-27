using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraKickBack : ICameraAction
{
    private CameraController cameraController;
    private CinemachineOrbitalFollow cameraOrbitalFreeLook => this.cameraController.cinemachineOrbitalFollow;
    public CameraKickBack(CameraController cameraController)
    {
        this.cameraController = cameraController;
    }
    public void Performed()
    {
        
    }
    public void Performed(Weapon weapon)
    {
        cameraController.StartCoroutine(KickUp(weapon.RecoilKickBack,weapon.RecoilCameraController));
    }
    float yAxisReposition;
    float repositionTime;
    IEnumerator KickUp(float kickForce,float controller)
    {
        yield return new WaitForFixedUpdate();
 
        yAxisReposition = cameraOrbitalFreeLook.VerticalAxis.Value;
        cameraOrbitalFreeLook.VerticalAxis.Value -= (kickForce-controller)*0.0004f;
        repositionTime = 0.22f;
        while (cameraOrbitalFreeLook.VerticalAxis.Value < yAxisReposition&&repositionTime>0)
        {
            cameraOrbitalFreeLook.VerticalAxis.Value += 0.12f*Time.deltaTime;
            repositionTime -= Time.deltaTime;
            yield return null;
        }

    }
}
