using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraKickBack : ICameraAction
{
    private CameraController cameraController;
    private Coroutine coroutine;
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
        if(this.coroutine != null)
        {
            cameraController.StopCoroutine(this.coroutine);
        }

       this.coroutine = cameraController.StartCoroutine(KickUp
           (Mathf.Lerp(cameraController.cameraKickPositionRange.x
           ,cameraController.cameraKickPositionRange.y,weapon.Recoil_Camera)
           )
           );
    }
    float pitchReposition;
    float repositionTime;
    IEnumerator KickUp(float kickForce)
    {
        yield return new WaitForFixedUpdate();
 
        pitchReposition = thirdPersonCam.pitch;
        thirdPersonCam.InputRotateCamera(0, - kickForce);
        repositionTime = 0.3f;
        while (thirdPersonCam.pitch > pitchReposition && repositionTime>0)
        {
            thirdPersonCam.InputRotateCamera(0, 0.5f * Time.deltaTime);
            repositionTime -= Time.deltaTime;
            yield return null;
        }

    }
}
