using System;
using UnityEngine;

public class DrawPrimaryWeaponManuverNodeLeaf : WeaponManuverLeafNode
{
    private float duration = 0.26f;
    private float elapesTime;
    public DrawPrimaryWeaponManuverNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
    }

    public override void Enter()
    {
        elapesTime = 0;
        (weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon).AttatchWeaponTo(weaponAdvanceUser);
        weaponAdvanceUser.weaponAfterAction.SwitchingWeapon(weaponAdvanceUser.currentWeapon,this);
    }

    public override void Exit()
    {
       
    }

    public override void FixedUpdateNode()
    {
        
    }
    public override bool IsReset()
    {
        return IsComplete();
    }
    public override bool IsComplete()
    {
        return elapesTime >= duration;
    }

    public override void UpdateNode()
    {
        this.elapesTime += Time.deltaTime;
    }
}
