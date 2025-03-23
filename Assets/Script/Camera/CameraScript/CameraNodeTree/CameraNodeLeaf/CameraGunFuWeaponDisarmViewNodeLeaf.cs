using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraGunFuWeaponDisarmViewNodeLeaf : CameraNodeLeaf
{
    private CinemachineCameraOffset cameraOffset => cameraController.cameraOffset;
    private CinemachineCamera cinemachineFreeLook => cameraController.cinemachineCamera;

    private float speedEnterView = 6f;
    public CameraGunFuWeaponDisarmViewNodeLeaf(CameraController cameraController, Func<bool> preCondition) : base(cameraController, preCondition)
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
                viewAttribute.WeaponDisarmed_Offset_Right.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.unscaledDeltaTime);

        }
        else //this.cameraController.curSide == CameraController.Side.left
        {
            offsetX = Mathf.Lerp(this.cameraOffset.Offset.x,
                -viewAttribute.WeaponDisarmed_Offset_Right.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.unscaledDeltaTime);
        }

        this.cinemachineFreeLook.Lens.FieldOfView = Mathf.Lerp(this.cinemachineFreeLook.Lens.FieldOfView, viewAttribute.WeaponDisarmed_FOV, speedEnterView * Time.unscaledDeltaTime);

        float offsetY = Mathf.Lerp(this.cameraOffset.Offset.y, viewAttribute.WeaponDisarmed_Offset_Right.y, speedEnterView * Time.unscaledDeltaTime);
        float offsetZ = Mathf.Lerp(this.cameraOffset.Offset.z, viewAttribute.WeaponDisarmed_Offset_Right.z, speedEnterView * Time.unscaledDeltaTime);

        this.cameraOffset.Offset = new Vector3(offsetX, offsetY, offsetZ);

        base.UpdateNode();
    }
}
