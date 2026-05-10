using System;
using UnityEngine;

public class AimDownSightBodyRotationConstraintNodeLeaf : BodyRotationConstraintNodeLeaf
{
    protected Transform aimAtPosition;
    protected Transform bodyAimRootPosRef;
    protected Transform bodyAimRootDirRef;
    protected IRangeWeaponAdvanceUser weaponAdvanceUser;

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
        float w = Mathf.MoveTowards(this.bodyConstraint.GetWeight(), this.weaponAdvanceUser._weaponManuverManager.aimingWeight, Time.deltaTime * 3);
        this.bodyConstraint.SetWeight(w);
    }
}
