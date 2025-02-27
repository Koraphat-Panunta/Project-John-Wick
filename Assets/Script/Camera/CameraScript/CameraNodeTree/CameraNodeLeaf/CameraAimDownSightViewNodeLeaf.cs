using Unity.Cinemachine;
using System;
using UnityEngine;

public class CameraAimDownSightViewNodeLeaf : CameraNodeLeaf
{
    private CinemachineCameraOffset cinemachineOffset => base.cameraController.cameraOffset;
    private CinemachineCamera cinemachineCamera => base.cameraController.cinemachineCamera;


    public CameraAimDownSightViewNodeLeaf(CameraController cameraController, Func<bool> preCondition) : base(cameraController, preCondition)
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
                viewAttribute.AimDownSight_Offset_Right.x,
                this.cameraController.cameraSwitchSholderVelocity*Time.deltaTime);
           
        }
        else //this.cameraController.curSide == CameraController.Side.left
        {
            offsetX = Mathf.Lerp(this.cinemachineOffset.Offset.x,
                - viewAttribute.AimDownSight_Offset_Right.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.deltaTime);
        }

        this.cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(this.cinemachineCamera.Lens.FieldOfView, cameraController.cameraViewAttribute.AimDownSight_FOV, cameraController.zoomingWeight);

        float offsetY = Mathf.Lerp(this.cinemachineOffset.Offset.y, viewAttribute.AimDownSight_Offset_Right.y, this.cameraController.zoomingWeight);
        float offsetZ = Mathf.Lerp(this.cinemachineOffset.Offset.z, viewAttribute.AimDownSight_Offset_Right.z, this.cameraController.zoomingWeight);

        this.cinemachineOffset.Offset = new Vector3(offsetX, offsetY, offsetZ);

        base.UpdateNode();
    }
}
