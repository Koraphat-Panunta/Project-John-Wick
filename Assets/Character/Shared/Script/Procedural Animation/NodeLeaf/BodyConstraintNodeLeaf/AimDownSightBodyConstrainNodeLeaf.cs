using System;
using UnityEngine;

public class AimDownSightBodyConstrainNodeLeaf : LookBodyConstraintNodeLeaf
{
    protected Transform aimAtPosition;
    protected IWeaponAdvanceUser weaponAdvanceUser;
    public AimDownSightBodyConstrainNodeLeaf(Transform aimAtPosition
        , IWeaponAdvanceUser weaponAdvanceUser
        , BodyLookConstrain splineLookConstrain
        ,AimBodyConstrainScriptableObject aimSplineLookConstrainScriptableObject
        ,Func<bool> precondition) : base(splineLookConstrain,aimSplineLookConstrainScriptableObject,precondition)
    {
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
        base.bodyLookConstrain.SetLookAtPosition(this.aimAtPosition.position);
    }

    protected override void UpdateWeight()
    {
        base.bodyLookConstrain.SetWeight(this.weaponAdvanceUser._weaponManuverManager.aimingWeight);
    }
}
