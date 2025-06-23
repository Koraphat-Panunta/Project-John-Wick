using Unity.Cinemachine;
using System;
using UnityEngine;

public class CameraAimDownSightViewNodeLeaf : CameraThirdPersonControllerViewNodeLeaf
{
    private Vector3 cinemachineOffset => base.cameraController.thirdPersonCinemachineCamera.cameraOffset;
    private CinemachineCamera cinemachineCamera => base.cameraController.cinemachineCamera;
    private ThirdPersonCinemachineCamera thirdPersonCamera => base.cameraController.thirdPersonCinemachineCamera;
    private Vector2 inputLook => cameraController.player.inputLookDir_Local * Time.deltaTime * cameraController.aimDownSightCameraSensivity;

    public CameraAimDownSightViewNodeLeaf(CameraController cameraController,CameraThirdPersonControllerViewScriptableObject cameraThirdPersonControllerViewScriptableObject, Func<bool> preCondition)
        : base(cameraController,cameraThirdPersonControllerViewScriptableObject, preCondition)
    {

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
        CameraThirdPersonControllerViewScriptableObject viewAttribute = this.cameraController.cameraAimDownSightView_SCRP;
        float offsetX;

        thirdPersonCamera.InputRotateCamera(inputLook.x,-inputLook.y);
        thirdPersonCamera.UpdateCameraPosition();

        if (this.cameraController.curSide == Player.ShoulderSide.Right)
        {
            offsetX = Mathf.Lerp(cameraController.thirdPersonCinemachineCamera.cameraOffset.x, 
                viewAttribute.viewOffsetRight.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.deltaTime);
           
        }
        else //this.cameraController.curSide == CameraController.Side.left
        {
            offsetX = Mathf.Lerp(cameraController.thirdPersonCinemachineCamera.cameraOffset.x,
                - viewAttribute.viewOffsetRight.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.deltaTime);
        }

        this.cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(base.enteringFOV, viewAttribute.fov, cameraController.zoomingWeight);

        float offsetY = Mathf.Lerp(this.enteringOffset.y, viewAttribute.viewOffsetRight.y, this.cameraController.zoomingWeight);
        float offsetZ = Mathf.Lerp(this.enteringOffset.z, viewAttribute.viewOffsetRight.z, this.cameraController.zoomingWeight);

        cameraController.thirdPersonCinemachineCamera.cameraOffset = new Vector3(offsetX, offsetY, offsetZ);
    }
}
