using System;
using UnityEngine;

public class AimDownSightBodyConstrainNodeLeaf : LookBodyConstraintNodeLeaf
{
    protected Transform aimAtPosition;
    protected Transform bodyAimRootPosRef;
    protected Transform bodyAimRootDirRef;
    protected IWeaponAdvanceUser weaponAdvanceUser;

    protected float maxHorizontalAngleDeg => this.aimSplineLookConstrainScriptableObject.maxHorizontalDeg;
    protected float maxVerticalAngleDeg => this.aimSplineLookConstrainScriptableObject.maxVerticalDeg;
    public AimDownSightBodyConstrainNodeLeaf(
        Transform bodyAimRootPosRef
        , Transform bodyAimRefDir
        , Transform aimAtPosition
        , IWeaponAdvanceUser weaponAdvanceUser
        , BodyLookConstrain splineLookConstrain
        ,AimBodyConstrainScriptableObject aimSplineLookConstrainScriptableObject
        ,Func<bool> precondition) : base(splineLookConstrain,aimSplineLookConstrainScriptableObject,precondition)
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
        Vector3 lookAtDir = ClampDirection.GetClampDirection(
           this.bodyAimRootDirRef.forward
           , (this.aimAtPosition.position - this.bodyAimRootPosRef.position).normalized
           , this.maxHorizontalAngleDeg
           , this.maxVerticalAngleDeg
           );

        Vector3 lookAtPos = this.bodyAimRootPosRef.position + (lookAtDir * 10);
        Debug.DrawLine(this.bodyAimRootPosRef.position, lookAtPos,Color.blue);

        this.bodyLookConstrain.SetLookAtPosition(Vector3.Lerp(this.bodyLookConstrain.bodyLookAtPosition.position,lookAtPos,Time.deltaTime * 5));
    }

    protected override void UpdateWeight()
    {
        base.bodyLookConstrain.SetWeight(this.weaponAdvanceUser._weaponManuverManager.aimingWeight);
    }
}
