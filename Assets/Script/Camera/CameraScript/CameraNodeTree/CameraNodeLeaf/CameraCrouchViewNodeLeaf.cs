using Cinemachine;
using System;
using UnityEngine;

public class CameraCrouchViewNodeLeaf : CameraNodeLeaf
{
    private CinemachineCameraOffset cinemachineOffset;
    private CinemachineFreeLook cinemachineFreeLook;

    private float elapseTime;
    private float speedEnterView = 3;
    public CameraCrouchViewNodeLeaf(CameraController cameraController, Func<bool> preCondition) : base(cameraController, preCondition)
    {
        this.cinemachineOffset = cameraController.cameraOffset;
        this.cinemachineFreeLook = cameraController.CinemachineFreeLook;
    }
    public override void Enter()
    {
        elapseTime = 0;
        base.Enter();
    }

    public override void Exit()
    {
        elapseTime = 0;
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        elapseTime += Time.deltaTime * speedEnterView;
        elapseTime = Mathf.Clamp01(elapseTime);

        this.cinemachineOffset.m_Offset = Vector3.Lerp(this.cinemachineOffset.m_Offset, cameraController.cameraViewAttribute.CrouchView_Offset_Right, elapseTime);
        this.cinemachineFreeLook.m_Lens.FieldOfView = Mathf.Lerp(this.cinemachineFreeLook.m_Lens.FieldOfView, cameraController.cameraViewAttribute.CrouchView_FOV, elapseTime);

        base.UpdateNode();
    }
}
