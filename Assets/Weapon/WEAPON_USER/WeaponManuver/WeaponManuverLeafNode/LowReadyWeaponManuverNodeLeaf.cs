using System;
using UnityEngine;

public class LowReadyWeaponManuverNodeLeaf : WeaponManuverLeafNode
{
    WeaponManuverManager weaponManuverManager;
    WeaponAfterAction weaponAfterAction;
    Weapon curWeapon => weaponAdvanceUser._currentWeapon;

    public float recoverFormAimDownSight {private get; set; }
    public LowReadyWeaponManuverNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        this.weaponAfterAction = weaponAdvanceUser.weaponAfterAction;
        recoverFormAimDownSight = 2;
    }

    public override void Enter()
    {
        this.weaponAfterAction.LowReady(curWeapon);
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdateNode()
    {
        weaponManuverManager.aimingWeight = Mathf.Clamp01(weaponManuverManager.aimingWeight - Time.deltaTime * recoverFormAimDownSight);
    }

    public override bool IsComplete()
    {
        if (weaponManuverManager.aimingWeight <= 0)
            return true;
        return false;
    }

    public override void UpdateNode()
    {
        if (this.weaponManuverManager == null)
            this.weaponManuverManager = weaponAdvanceUser.weaponManuverManager;

        if (this.weaponManuverManager.isReloadManuver)
            curWeapon.Reload();

        this.weaponAfterAction.LowReady(curWeapon);
    }
}
