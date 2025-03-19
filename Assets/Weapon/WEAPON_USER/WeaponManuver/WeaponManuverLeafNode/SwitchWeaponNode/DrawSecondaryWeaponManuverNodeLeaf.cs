using System;
using UnityEngine;

public class DrawSecondaryWeaponManuverNodeLeaf : WeaponManuverLeafNode
{
    private float duration = 0.25f;
    private float elapseTime;
    public DrawSecondaryWeaponManuverNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
    }

    public override void Enter()
    {
        elapseTime = 0;
        (weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon).AttatchWeaponTo(weaponAdvanceUser);
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdateNode()
    {
        
    }

    public override bool IsComplete()
    {
        return elapseTime > duration;
    }

    public override void UpdateNode()
    {
        elapseTime += Time.deltaTime;
    }
}
