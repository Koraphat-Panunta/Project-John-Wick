using System;
using UnityEngine;

public class WeaponRestNodeLeaf : WeaponLeafNode
{
    public WeaponRestNodeLeaf(Weapon weapon, Func<bool> preCondition) : base(weapon, preCondition)
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
        Weapon.Notify(Weapon, WeaponSubject.WeaponNotifyType.Rest);
    }

    public override void Exit()
    {
        
    }

   
}
