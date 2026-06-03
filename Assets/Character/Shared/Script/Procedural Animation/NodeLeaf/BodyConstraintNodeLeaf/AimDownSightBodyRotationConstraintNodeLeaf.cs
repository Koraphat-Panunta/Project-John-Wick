using System;
using UnityEngine;

public class AimDownSightBodyRotationConstraintNodeLeaf : BodyRotationConstraintNodeLeaf
{
    protected Transform aimAtPosition;
    protected Transform bodyAimRootPosRef;
    protected Transform bodyAimRootDirRef;
    protected IRangeWeaponAdvanceUser weaponAdvanceUser;

    public override Vector3 getOffsetConstraint => this.bodyRecoilModifier.ApplyOffset(base.getOffsetConstraint,this.bodyRecoilSCRP,this.weight);
    public override Vector3 getOffsetConstraint1 => this.bodyRecoilModifier.ApplyOffset(base.getOffsetConstraint1, this.bodyRecoilSCRP, this.weight);
    public override Vector3 getOffsetConstraint2 => this.bodyRecoilModifier.ApplyOffset(base.getOffsetConstraint2, this.bodyRecoilSCRP, this.weight);

    public BodyRecoilModifier bodyRecoilModifier { get; protected set; }
    public BodyRecoilSCRP bodyRecoilSCRP { get; protected set; }

    protected float maxHorizontalAngleDeg => this.bodyRotationConstrainScriptableObject.maxHorizontalDeg;
    protected float maxVerticalAngleDeg => this.bodyRotationConstrainScriptableObject.maxVerticalDeg;

    public AimDownSightBodyRotationConstraintNodeLeaf(
        Transform bodyAimRootPosRef
        , Transform bodyAimRefDir
        , Transform aimAtPosition
        , IRangeWeaponAdvanceUser weaponAdvanceUser
        , BodyConstraintManager bodyConstraint
        , BodyRotationConstrainScriptableObject bodyRotationConstrainScriptableObject
        , Func<bool> precondition) : base(bodyConstraint, bodyRotationConstrainScriptableObject, precondition)
    {
        this.bodyAimRootPosRef = bodyAimRootPosRef;
        this.bodyAimRootDirRef = bodyAimRefDir;
        this.aimAtPosition = aimAtPosition;
        this.weaponAdvanceUser = weaponAdvanceUser;

        this.bodyRecoilModifier = new BodyRecoilModifier();
    }

    public override void Enter()
    {
        this.bodyRecoilModifier.Reset();
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

    protected override void UpdateLookAtTarget()
    {
        this.bodyRecoilModifier.UpdateWeights(this.bodyRecoilSCRP);

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

    public void TriggerRecoil()
    {
        this.bodyRecoilModifier.Trigger();
    }

    public void SetRecoilScriptableObject(BodyRecoilSCRP bodyRecoilSCRP)
    {
        this.bodyRecoilSCRP = bodyRecoilSCRP;
    }

    protected override void UpdateWeight()
    {
        float w = Mathf.MoveTowards(this.bodyConstraint.GetWeight(), this.weaponAdvanceUser._weaponManuverManager.aimingWeight, Time.deltaTime * 3);
        this.weight = w;
        this.bodyConstraint.SetWeight(w);
    }
}
