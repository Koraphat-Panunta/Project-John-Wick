using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraGunFuExecuteOnGroundNodeLeaf : CameraNodeLeaf
{
    private CinemachineCameraOffset cameraOffset => cameraController.cameraOffset;
    private CinemachineCamera cinemachineFreeLook => cameraController.cinemachineCamera;

    private float speedEnterView = 7f;
    public CameraGunFuExecuteOnGroundNodeLeaf(CameraController cameraController, Func<bool> preCondition) : base(cameraController, preCondition)
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
            offsetX = Mathf.Lerp(this.cameraOffset.Offset.x,
                viewAttribute.GunFuExecute_Offset_Right.x,
                speedEnterView * Time.deltaTime);

        }
        else //this.cameraController.curSide == CameraController.Side.left
        {
            offsetX = Mathf.Lerp(this.cameraOffset.Offset.x,
                -viewAttribute.GunFuExecute_Offset_Right.x,
                speedEnterView * Time.deltaTime);
        }

        this.cinemachineFreeLook.Lens.FieldOfView = Mathf.Lerp(this.cinemachineFreeLook.Lens.FieldOfView, viewAttribute.GunFuExecute_FOV, speedEnterView * Time.deltaTime);

        float offsetY = Mathf.Lerp(this.cameraOffset.Offset.y, viewAttribute.GunFuExecute_Offset_Right.y, speedEnterView * Time.deltaTime);
        float offsetZ = Mathf.Lerp(this.cameraOffset.Offset.z, viewAttribute.GunFuExecute_Offset_Right.z, speedEnterView * Time.deltaTime);

        this.cameraOffset.Offset = new Vector3(offsetX, offsetY, offsetZ);

        base.UpdateNode();
    }
}
