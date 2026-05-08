using System;
using UnityEngine;

public class WeaponRestNodeLeaf : WeaponLeafNode
{
    public WeaponRestNodeLeaf(RangeWeapon weapon, Func<bool> preCondition) : base(weapon, preCondition)
    {
    }
    public override void FixedUpdateNode()
    {

    }

    public override void UpdateNode()
    {

    }
    public override void Enter()
    {
        Weapon.Notify(Weapon, RangeWeaponSubject.WeaponNotifyType.Rest);
    }

    public override void Exit()
    {
        
    }

   
}
