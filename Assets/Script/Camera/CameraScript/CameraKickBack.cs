using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraKickBack : ICameraAction
{
    private CameraController cameraController;
    private CinemachineFreeLook cameraFreeLook => this.cameraController.CinemachineFreeLook;
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
 
        yAxisReposition = cameraFreeLook.m_YAxis.Value;
        cameraFreeLook.m_YAxis.Value -= (kickForce-controller)*0.0004f;
        repositionTime = 0.22f;
        while (cameraFreeLook.m_YAxis.Value < yAxisReposition&&repositionTime>0)
        {
            cameraFreeLook.m_YAxis.Value += 0.12f*Time.deltaTime;
            repositionTime -= Time.deltaTime;
            yield return null;
        }

    }
}
