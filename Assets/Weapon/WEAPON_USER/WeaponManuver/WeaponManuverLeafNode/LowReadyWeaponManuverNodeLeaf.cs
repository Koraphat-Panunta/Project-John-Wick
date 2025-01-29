using System;
using UnityEngine;

public class LowReadyWeaponManuverNodeLeaf : WeaponManuverLeafNode
{
    WeaponManuverManager weaponManuverManager;
    public LowReadyWeaponManuverNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        this.weaponManuverManager = weaponAdvanceUser.weaponManuverManager;
    }

    public override void FixedUpdateNode()
    {
        weaponManuverManager.aimingWeight = Mathf.Clamp01(weaponManuverManager.aimingWeight - Time.deltaTime);
    }

    public override bool IsComplete()
    {
        if (weaponManuverManager.aimingWeight <= 0)
            return true;
        return false;
    }

    public override void UpdateNode()
    {

    }
}
