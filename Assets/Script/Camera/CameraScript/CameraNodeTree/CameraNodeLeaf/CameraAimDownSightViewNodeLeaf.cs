using Cinemachine;
using System;
using UnityEngine;

public class CameraAimDownSightViewNodeLeaf : CameraNodeLeaf
{
    private CinemachineCameraOffset cinemachineOffset;
    private CinemachineFreeLook cinemachineFreeLook;


    public CameraAimDownSightViewNodeLeaf(CameraController cameraController, Func<bool> preCondition) : base(cameraController, preCondition)
    {
        this.cinemachineOffset = cameraController.cameraOffset;
        this.cinemachineFreeLook = cameraController.CinemachineFreeLook;
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
        if (this.cameraController.curSide == CameraController.Side.right)
        {
            this.cinemachineOffset.m_Offset = Vector3.Lerp(this.cinemachineOffset.m_Offset, cameraController.cameraViewAttribute.AimDownSight_Offset_Right, cameraController.zoomingWeight);
            this.cinemachineFreeLook.m_Lens.FieldOfView = Mathf.Lerp(this.cinemachineFreeLook.m_Lens.FieldOfView, cameraController.cameraViewAttribute.AimDownSight_FOV, cameraController.zoomingWeight);
        }
        if (this.cameraController.curSide == CameraController.Side.left)
        {
            this.cinemachineOffset.m_Offset = Vector3.Lerp(this.cinemachineOffset.m_Offset, cameraController.cameraViewAttribute.AimDownSight_Offset_Left, cameraController.zoomingWeight);
            this.cinemachineFreeLook.m_Lens.FieldOfView = Mathf.Lerp(this.cinemachineFreeLook.m_Lens.FieldOfView, cameraController.cameraViewAttribute.AimDownSight_FOV, cameraController.zoomingWeight);
        }

        base.UpdateNode();
    }
}
