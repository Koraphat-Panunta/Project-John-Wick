using System;
using UnityEngine;

public class CameraGunFuHitViewNodeLeaf : CameraNodeLeaf
{
    CinemachineCameraOffset offset;
    public CameraGunFuHitViewNodeLeaf(CameraController cameraController, Func<bool> preCondition) : base(cameraController, preCondition)
    {
        this.offset = cameraController.cameraOffset;
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
        base.UpdateNode();
    }
}
