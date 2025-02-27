using Unity.Cinemachine;
using System;
using UnityEngine;

public class CameraStandViewNodeLeaf : CameraNodeLeaf
{
    private CinemachineCameraOffset cinemachineOffset => base.cameraController.cameraOffset;
    private CinemachineCamera cinemachineFreeLook => base.cameraController.cinemachineCamera;

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

        if (this.cameraController.curSide == Player.ShoulderSide.Right)
        {
            offsetX = Mathf.Lerp(this.cinemachineOffset.Offset.x,
                viewAttribute.StandView_Offset_Right.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.deltaTime);

        }
        else //this.cameraController.curSide == CameraController.Side.left
        {
            offsetX = Mathf.Lerp(this.cinemachineOffset.Offset.x,
                -viewAttribute.StandView_Offset_Right.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.deltaTime);
        }

        this.cinemachineFreeLook.Lens.FieldOfView = Mathf.Lerp(this.cinemachineFreeLook.Lens.FieldOfView, viewAttribute.StandView_FOV, speedEnterView * Time.deltaTime);

        float offsetY = Mathf.Lerp(this.cinemachineOffset.Offset.y, viewAttribute.StandView_Offset_Right.y, speedEnterView * Time.deltaTime);
        float offsetZ = Mathf.Lerp(this.cinemachineOffset.Offset.z, viewAttribute.StandView_Offset_Right.z, speedEnterView * Time.deltaTime);

        this.cinemachineOffset.Offset = new Vector3(offsetX, offsetY, offsetZ);
        base.UpdateNode();
    }
}
