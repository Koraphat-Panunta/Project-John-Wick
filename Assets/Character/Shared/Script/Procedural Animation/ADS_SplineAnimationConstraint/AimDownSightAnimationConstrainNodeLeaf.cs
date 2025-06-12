using System;
using UnityEngine;

public class AimDownSightAnimationConstrainNodeLeaf : AnimationConstrainNodeLeaf
{
    private IWeaponAdvanceUser weaponAdvanceUser;
    private SplineLookConstrain splineLookConstrain;
    private float weight => weaponAdvanceUser.weaponManuverManager.aimingWeight;
    private AimSplineLookConstrainScriptableObject aimSplineLookConstrainScriptableObject;
    public AimDownSightAnimationConstrainNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser
        ,SplineLookConstrain splineLookConstrain
        ,AimSplineLookConstrainScriptableObject aimSplineLookConstrainScriptableObject
        ,Func<bool> precondition) : base(precondition)
    {
        this.weaponAdvanceUser = weaponAdvanceUser;
        this.splineLookConstrain = splineLookConstrain;
        this.aimSplineLookConstrainScriptableObject = aimSplineLookConstrainScriptableObject;
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
        splineLookConstrain.SetWeight(weight, aimSplineLookConstrainScriptableObject);
        base.UpdateNode();
    }
}
