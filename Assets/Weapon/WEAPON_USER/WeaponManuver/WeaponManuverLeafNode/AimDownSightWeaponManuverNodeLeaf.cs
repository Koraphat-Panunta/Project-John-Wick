using System;
using UnityEngine;

public class AimDownSightWeaponManuverNodeLeaf : WeaponManuverLeafNode
{
    WeaponManuverManager weaponManuverManager;
    Weapon curWeapon;
    public AimDownSightWeaponManuverNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        this.weaponManuverManager = weaponAdvanceUser.weaponManuverManager;
        curWeapon = weaponManuverManager.curWeapon;
    }

    public override void FixedUpdateNode()
    {
        weaponManuverManager.aimingWeight = Mathf.Clamp01(weaponManuverManager.aimingWeight + Time.deltaTime);
    }

    public override bool IsComplete()
    {
        if(weaponManuverManager.aimingWeight >= 1)
            return true;

        return false;
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override bool Precondition()
    {
        return base.Precondition();
    }

    public override void UpdateNode()
    {
        if (weaponManuverManager.isReload)
        {
            
        }
    }
}
