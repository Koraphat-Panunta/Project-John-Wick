using System;
using UnityEngine;

public class AimDownSightBodyConstrainNodeLeaf : LookBodyConstraintNodeLeaf
{
    private IWeaponAdvanceUser weaponAdvanceUser;

    public AimDownSightBodyConstrainNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser
        ,BodyLookConstrain splineLookConstrain
        ,AimBodyConstrainScriptableObject aimSplineLookConstrainScriptableObject
        ,Func<bool> precondition) : base(splineLookConstrain,aimSplineLookConstrainScriptableObject,precondition)
    {
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
        base.bodyLookConstrain.SetLookAtPosition(weaponAdvanceUser._pointingPos);
    }

    protected override void UpdateWeight()
    {
        base.bodyLookConstrain.SetWeight(weaponAdvanceUser._weaponManuverManager.aimingWeight);
    }
}
