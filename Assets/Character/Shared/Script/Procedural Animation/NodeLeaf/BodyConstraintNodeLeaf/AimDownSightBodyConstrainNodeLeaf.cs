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
        , BodyLookConstrainManager splineLookConstrain
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

        Vector3 refDir = Quaternion.LookRotation(this.bodyAimRootDirRef.forward, this.bodyAimRootDirRef.up) * Quaternion.Euler(this.aimSplineLookConstrainScriptableObject.rotateRefDirOffset) * Vector3.forward;

        Debug.DrawRay(this.bodyAimRootDirRef.position, refDir * 2, Color.yellow);

        Vector3 lookAtDir = ClampDirection.GetClampDirection(
           refDir
           , (this.aimAtPosition.position - this.bodyAimRootPosRef.position).normalized
           ,Vector3.up  
           , this.maxHorizontalAngleDeg
           , this.maxVerticalAngleDeg
           );

        Vector3 lookAtPos = this.bodyAimRootPosRef.position + (lookAtDir * 2 );
        Debug.DrawLine(this.bodyAimRootPosRef.position, lookAtPos,Color.blue);

        this.bodyLookConstrain.SetLookAtPosition(lookAtPos);
    }

    protected override void UpdateWeight()
    {
        base.bodyLookConstrain.SetWeight(this.weaponAdvanceUser._weaponManuverManager.aimingWeight);
    }
}
