using Unity.Cinemachine;
using System;
using UnityEngine;

public class CameraAimDownSightViewNodeLeaf : CameraThirdPersonControllerViewNodeLeaf
{
    private Vector3 cinemachineOffset => base.cameraController.thirdPersonCinemachineCamera.cameraOffset;
    private CinemachineCamera cinemachineCamera => base.cameraController.cinemachineCamera;
    private ThirdPersonCinemachineCamera thirdPersonCamera => base.cameraController.thirdPersonCinemachineCamera;
    private Vector2 inputLook => Vector2.Lerp(
        cameraController.player.inputLookDir_Local * Time.deltaTime * cameraController.standardCameraSensivity
        , cameraController.player.inputLookDir_Local * Time.deltaTime * cameraController.aimDownSightCameraSensivity
        ,cameraController.player.weaponAdvanceUser._weaponManuverManager.aimingWeight) ;
    private float restOffsetZ;
    public CameraAimDownSightViewNodeLeaf(CameraController cameraController,CameraThirdPersonControllerViewScriptableObject cameraThirdPersonControllerViewScriptableObject,float restOffsetZ, Func<bool> preCondition)
        : base(cameraController,cameraThirdPersonControllerViewScriptableObject, preCondition)
    {
        this.restOffsetZ = restOffsetZ;
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        normalizedTime = Mathf.Clamp01(normalizedTime += Time.deltaTime * cameraThirdPersonControllerViewScriptableObject.transitionInSpeed);
        float offsetX;

        thirdPersonCamera.InputRotateCamera(inputLook.x,-inputLook.y);
        thirdPersonCamera.UpdateCameraPosition();

        if (this.cameraController.curSide == Player.ShoulderSide.Right)
        {
            offsetX = Mathf.Lerp(cameraController.thirdPersonCinemachineCamera.cameraOffset.x, 
                base.cameraThirdPersonControllerViewScriptableObject.viewOffsetRight.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.deltaTime);
           
        }
        else //this.cameraController.curSide == CameraController.Side.left
        {
            offsetX = Mathf.Lerp(cameraController.thirdPersonCinemachineCamera.cameraOffset.x,
                - base.cameraThirdPersonControllerViewScriptableObject.viewOffsetRight.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.deltaTime);
        }

        this.cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(base.enteringFOV, base.cameraThirdPersonControllerViewScriptableObject.fov, cameraController.zoomingWeight);

        float offsetY = Mathf.Lerp(this.enteringOffset.y, base.cameraThirdPersonControllerViewScriptableObject.viewOffsetRight.y, base.normalizedTime);
        float offsetZ = Mathf.Lerp(this.restOffsetZ, base.cameraThirdPersonControllerViewScriptableObject.viewOffsetRight.z, this.cameraController.zoomingWeight);

        cameraController.thirdPersonCinemachineCamera.cameraOffset = new Vector3(offsetX, offsetY, offsetZ);
    }
}
