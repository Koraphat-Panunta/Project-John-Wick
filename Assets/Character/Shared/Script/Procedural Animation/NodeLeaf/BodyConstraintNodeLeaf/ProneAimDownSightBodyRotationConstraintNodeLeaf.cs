using System;
using UnityEngine;

public class ProneAimDownSightBodyRotationConstraintNodeLeaf : BodyRotationConstraintNodeLeaf
{
    protected Transform aimAtPosition;
    protected Transform bodyAimRootPosRef;
    protected Transform bodyAimRootDirRef;
    protected IRangeWeaponAdvanceUser weaponAdvanceUser;

    public float angle;
    private BodyRotationScriptableObjectBlend bodyRotationBlendScrp;

    protected float maxHorizontalAngleDeg => this.bodyRotationConstrainScriptableObject.maxHorizontalDeg;
    protected float maxVerticalAngleDeg => this.bodyRotationConstrainScriptableObject.maxVerticalDeg;

    public ProneAimDownSightBodyRotationConstraintNodeLeaf(
        Transform bodyAimRootPosRef
        , Transform bodyAimRefDir
        , Transform aimAtPosition
        , IRangeWeaponAdvanceUser weaponAdvanceUser
        , BodyConstraintManager bodyConstraint
        , BodyRotationScriptableObjectBlend bodyRotationBlendScrp
        , Func<bool> precondition) : base(bodyConstraint, bodyRotationBlendScrp, precondition)
    {
        this.bodyAimRootPosRef = bodyAimRootPosRef;
        this.bodyAimRootDirRef = bodyAimRefDir;
        this.aimAtPosition = aimAtPosition;
        this.weaponAdvanceUser = weaponAdvanceUser;
        this.bodyRotationBlendScrp = bodyRotationBlendScrp;
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
        this.bodyRotationBlendScrp.GetBlendData(this.angle);
        base.UpdateNode();
    }

    protected override void UpdateLookAtTarget()
    {
        Vector3 refDir = Quaternion.LookRotation(this.bodyAimRootDirRef.forward, this.bodyAimRootDirRef.up)
            * Quaternion.Euler(this.bodyRotationConstrainScriptableObject.rotateRefDirOffset)
            * Vector3.forward;

        Vector3 lookAtDir = ClampDirection.GetClampDirection(
            refDir,
            (this.aimAtPosition.position - this.bodyAimRootPosRef.position).normalized,
            Vector3.up,
            this.maxHorizontalAngleDeg,
            this.maxVerticalAngleDeg);

        this.bodyConstraint.SetLookPos(this.bodyAimRootPosRef.position + lookAtDir * 2);
    }

    protected override void UpdateWeight()
    {
        this.bodyConstraint.SetWeight(this.weaponAdvanceUser._weaponManuverManager.aimingWeight);
    }

    public void SetAngle(float angle) => this.angle = angle;

    public void SetBlendSCRP(BodyRotationScriptableObjectBlend scrp)
    {
        this.bodyRotationBlendScrp = scrp;
        this.SetBodyRotationConstrainSCRP(scrp);
    }
}
