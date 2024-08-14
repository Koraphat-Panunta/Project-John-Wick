using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraKickBack : ICameraAction
{
    private CameraController cameraController;
    private CinemachineFreeLook cameraFreeLook;
    public CameraKickBack(CameraController cameraController)
    {
        this.cameraController = cameraController;
        cameraFreeLook = this.cameraController.CinemachineFreeLook;
    }
    public void Performed()
    {
        
    }
    public void Performed(Weapon weapon)
    {
        cameraController.StartCoroutine(KickUp(weapon.RecoilKickBack,weapon.RecoilController));
    }
    float yAxisReposition;
    float repositionTime;
    IEnumerator KickUp(float kickForce,float kickController)
    {
        yield return new WaitForFixedUpdate();
        float kickResult = (kickForce - kickController)/kickForce;
        kickResult = kickResult * 0.65f;
        yAxisReposition = cameraFreeLook.m_YAxis.Value;
        cameraFreeLook.m_YAxis.Value -= kickResult;
        repositionTime = 0.25f;
        while (cameraFreeLook.m_YAxis.Value < yAxisReposition&&repositionTime>0)
        {
            cameraFreeLook.m_YAxis.Value += 0.25f*Time.deltaTime;
            repositionTime -= Time.deltaTime;
            yield return null;
        }

    }
}
