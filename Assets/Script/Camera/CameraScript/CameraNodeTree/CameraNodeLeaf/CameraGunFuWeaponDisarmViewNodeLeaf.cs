using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraGunFuWeaponDisarmViewNodeLeaf : CameraNodeLeaf
{
    private Vector3 cameraOffset => cameraController.thirdPersonCinemachineCamera.cameraOffset;
    private CinemachineCamera cinemachineFreeLook => cameraController.cinemachineCamera;
    private ThirdPersonCinemachineCamera thirdPersonCamera => base.cameraController.thirdPersonCinemachineCamera;
    private Vector2 inputLook => cameraController.player.inputLookDir_Local * Time.deltaTime * cameraController.standardCameraSensivity;

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

        thirdPersonCamera.InputRotateCamera(inputLook.x, -inputLook.y);
        thirdPersonCamera.UpdateCameraPosition();

        if (this.cameraController.curSide == Player.ShoulderSide.Right)
        {
            offsetX = Mathf.Lerp(this.cameraOffset.x,
                viewAttribute.WeaponDisarmed_Offset_Right.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.unscaledDeltaTime);

        }
        else //this.cameraController.curSide == CameraController.Side.left
        {
            offsetX = Mathf.Lerp(this.cameraOffset.x,
                -viewAttribute.WeaponDisarmed_Offset_Right.x,
                this.cameraController.cameraSwitchSholderVelocity * Time.unscaledDeltaTime);
        }

        this.cinemachineFreeLook.Lens.FieldOfView = Mathf.Lerp(this.cinemachineFreeLook.Lens.FieldOfView, viewAttribute.WeaponDisarmed_FOV, speedEnterView * Time.unscaledDeltaTime);

        float offsetY = Mathf.Lerp(this.cameraOffset.y, viewAttribute.WeaponDisarmed_Offset_Right.y, speedEnterView * Time.unscaledDeltaTime);
        float offsetZ = Mathf.Lerp(this.cameraOffset.z, viewAttribute.WeaponDisarmed_Offset_Right.z, speedEnterView * Time.unscaledDeltaTime);

        cameraController.thirdPersonCinemachineCamera.cameraOffset = new Vector3(offsetX, offsetY, offsetZ);

        base.UpdateNode();
    }
}
