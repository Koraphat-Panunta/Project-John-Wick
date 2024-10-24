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
        cameraController.StartCoroutine(KickUp(weapon.RecoilCameraKickBack));
    }
    float yAxisReposition;
    float repositionTime;
    IEnumerator KickUp(float kickForce)
    {
        yield return new WaitForFixedUpdate();
 
        yAxisReposition = cameraFreeLook.m_YAxis.Value;
        cameraFreeLook.m_YAxis.Value -= kickForce;
        repositionTime = 0.22f;
        while (cameraFreeLook.m_YAxis.Value < yAxisReposition&&repositionTime>0)
        {
            cameraFreeLook.m_YAxis.Value += 0.15f*Time.deltaTime;
            repositionTime -= Time.deltaTime;
            yield return null;
        }

    }
}
