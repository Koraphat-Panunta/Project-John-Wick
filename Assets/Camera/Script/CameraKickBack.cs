using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraKickBack : ICameraAction
{
    private CameraController cameraController;
    private ThirdPersonCinemachineCamera thirdPersonCam => this.cameraController.thirdPersonCinemachineCamera;
    public CameraKickBack(CameraController cameraController)
    {
        this.cameraController = cameraController;
    }
    public void Performed()
    {
        
    }
    public void Performed(Weapon weapon)
    {
        cameraController.StartCoroutine(KickUp(weapon.RecoilKickBack,weapon.Recoil_CameraControlController));
    }
    float pitchReposition;
    float repositionTime;
    IEnumerator KickUp(float kickForce,float controller)
    {
        yield return new WaitForFixedUpdate();
 
        pitchReposition = thirdPersonCam.pitch;
        thirdPersonCam.InputRotateCamera(0, -(kickForce - controller) * cameraController.cameraKickUpMultiple);
        repositionTime = 0.3f;
        while (thirdPersonCam.pitch > pitchReposition && repositionTime>0)
        {
            thirdPersonCam.InputRotateCamera(0, 0.5f * Time.deltaTime);
            repositionTime -= Time.deltaTime;
            yield return null;
        }

    }
}
