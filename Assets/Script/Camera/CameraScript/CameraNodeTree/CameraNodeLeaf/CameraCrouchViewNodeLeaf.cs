using Unity.Cinemachine;
using System;
using UnityEngine;

public class CameraCrouchViewNodeLeaf : CameraNodeLeaf
{
    private Vector3 cinemachineOffset => base.cameraController.thirdPersonCinemachineCamera.cameraOffset;
    private CinemachineCamera cinemachineCamera => base.cameraController.cinemachineCamera;
    private ThirdPersonCinemachineCamera thirdPersonCamera => base.cameraController.thirdPersonCinemachineCamera;
    private Vector2 inputLook => cameraController.player.inputLookDir_Local * Time.deltaTime * cameraController.standardCameraSensivity;

    private float speedEnterView = 3;
    public CameraCrouchViewNodeLeaf(CameraController cameraController, Func<bool> preCondition) : base(cameraController, preCondition)
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

        ScrpCameraViewAttribute viewAttribute = this.cameraController.cameraViewAttribute;
        float offsetX;

        thirdPersonCamera.InputRotateCamera(inputLook.x,-inputLook.y);
        thirdPersonCamera.UpdateCameraPosition();

        if (this.cameraController.curSide == Player.ShoulderSide.Right)
        {
            offsetX = Mathf.Lerp(this.cinemachineOffset.x,
                viewAttribute.CrouchView_Offset_Right.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.deltaTime);

        }
        else //this.cameraController.curSide == CameraController.Side.left
        {
            offsetX = Mathf.Lerp(this.cinemachineOffset.x,
                -viewAttribute.CrouchView_Offset_Right.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.deltaTime);
        }

        this.cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(this.cinemachineCamera.Lens.FieldOfView, viewAttribute.CrouchView_FOV, speedEnterView*Time.deltaTime);

        float offsetY = Mathf.Lerp(this.cinemachineOffset.y, viewAttribute.CrouchView_Offset_Right.y, speedEnterView * Time.deltaTime);
        float offsetZ = Mathf.Lerp(this.cinemachineOffset.z, viewAttribute.CrouchView_Offset_Right.z, speedEnterView * Time.deltaTime);

        base.cameraController.thirdPersonCinemachineCamera.cameraOffset = new Vector3(offsetX, offsetY, offsetZ);

        base.UpdateNode();
    }
}
