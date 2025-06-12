using Unity.Cinemachine;
using System;
using UnityEngine;

public class CameraStandViewNodeLeaf : CameraNodeLeaf
{
    private Vector3 cinemachineOffset => base.cameraController.thirdPersonCinemachineCamera.cameraOffset;
    private CinemachineCamera cinemachineFreeLook => base.cameraController.cinemachineCamera;
    private ThirdPersonCinemachineCamera thirdPersonCamera => base.cameraController.thirdPersonCinemachineCamera;
    private Vector2 inputLook => cameraController.player.inputLookDir_Local * Time.deltaTime * cameraController.standardCameraSensivity;

    private float speedEnterView = 3;

    public CameraStandViewNodeLeaf(CameraController cameraController, Func<bool> preCondition) : base(cameraController, preCondition)
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
                viewAttribute.StandView_Offset_Right.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.deltaTime);

        }
        else //this.cameraController.curSide == CameraController.Side.left
        {
            offsetX = Mathf.Lerp(this.cinemachineOffset.x,
                -viewAttribute.StandView_Offset_Right.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.deltaTime);
        }

        this.cinemachineFreeLook.Lens.FieldOfView = Mathf.Lerp(this.cinemachineFreeLook.Lens.FieldOfView, viewAttribute.StandView_FOV, speedEnterView * Time.deltaTime);

        float offsetY = Mathf.Lerp(this.cinemachineOffset.y, viewAttribute.StandView_Offset_Right.y, speedEnterView * Time.deltaTime);
        float offsetZ = Mathf.Lerp(this.cinemachineOffset.z, viewAttribute.StandView_Offset_Right.z, speedEnterView * Time.deltaTime);

        cameraController.thirdPersonCinemachineCamera.cameraOffset = new Vector3(offsetX, offsetY, offsetZ);
        base.UpdateNode();
    }
}
