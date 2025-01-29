using System;
using UnityEngine;

public class LowReadyWeaponManuverNodeLeaf : WeaponManuverLeafNode
{
    WeaponManuverManager weaponManuverManager;
    WeaponAfterAction weaponAfterAction;
    Weapon curWeapon;

    public float recoverFormAimDownSight {private get; set; }
    public LowReadyWeaponManuverNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        this.weaponManuverManager = weaponAdvanceUser.weaponManuverManager;
        this.weaponAfterAction = weaponAdvanceUser.weaponAfterAction;
        this.curWeapon = weaponAdvanceUser.currentWeapon;
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
        this.weaponAfterAction.LowReady(curWeapon);
    }
}
