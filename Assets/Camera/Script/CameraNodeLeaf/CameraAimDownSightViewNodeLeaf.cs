using Unity.Cinemachine;
using System;
using UnityEngine;

public class CameraAimDownSightViewNodeLeaf : CameraThirdPersonControllerViewNodeLeaf
{
    private IRangeWeaponAdvanceUser weaponAdvanceUser;
    private float aimingWeight => weaponAdvanceUser._weaponManuverManager.aimingWeight;

    private CameraThirdPersonControllerViewScriptableObject aimDownSightViewSCRP;
    private CameraThirdPersonControllerViewNodeLeaf lowReadyNode;

    public override Vector3 targetLookPos => Vector3.Lerp(this.lowReadyNode.LookPosition, this.thirdPersonCamera.targetLookTarget.position, this.aimingWeight);
    public override Vector3 targetTrackPos => Vector3.Lerp(this.lowReadyNode.TrackPosition, this.thirdPersonCamera.targetFollowTarget.position, this.aimingWeight);

    public override float trackingCruve
        => Mathf.Lerp(
            this.lowReadyNode.trackingCruve
            , this.aimDownSightViewSCRP.transitionCurve.Evaluate(normalizedTime)
            , this.aimingWeight);

    public override Vector3 targetOffset => Vector3.Lerp(
        this.lowReadyNode.targetOffset,
        this.aimDownSightViewSCRP.viewOffsetRight,
        this.aimingWeight);

    public override float targetFOV => Mathf.Lerp(
        this.lowReadyNode.targetFOV,
        this.aimDownSightViewSCRP.fov,
        this.aimingWeight);

    public override float transitionSpeed => Mathf.Lerp(
        this.lowReadyNode.transitionSpeed, 
        this.cameraThirdPersonControllerViewScriptableObject.transitionInSpeed,
        this.aimingWeight);

    public CameraAimDownSightViewNodeLeaf(
        CameraController cameraController,
        CameraThirdPersonControllerViewScriptableObject aimDownSightViewSCRP,
        CameraThirdPersonControllerViewNodeLeaf lowReadyNode,
        IRangeWeaponAdvanceUser weaponAdvanceUser,
        Func<bool> preCondition)
        : base(cameraController, aimDownSightViewSCRP, preCondition)
    {
        this.weaponAdvanceUser = weaponAdvanceUser;
        this.aimDownSightViewSCRP = aimDownSightViewSCRP;
        this.lowReadyNode = lowReadyNode;
    }

    public override void Enter()
    {
        this.lowReadyNode.Enter();
        base.Enter();
    }

    public override void UpdateNode()
    {
        this.lowReadyNode.NormalizedTimeUpdate();
        this.lowReadyNode.OffsetUpdate();
        this.lowReadyNode.FOVUpdate();
        base.UpdateNode();
    }

    public override void FixedUpdateNode()
    {
        this.lowReadyNode.TrackPosUpdate();
        this.lowReadyNode.LookPosUpdate();
    }

    public override void Exit()
    {
        this.lowReadyNode.Exit();
        base.Exit();
    }


}
