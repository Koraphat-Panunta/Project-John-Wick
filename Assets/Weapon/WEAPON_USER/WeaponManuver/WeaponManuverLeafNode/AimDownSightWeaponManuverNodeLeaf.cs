using System;
using UnityEngine;

public class AimDownSightWeaponManuverNodeLeaf : WeaponManuverLeafNode
{
    WeaponManuverManager weaponManuverManager;
    WeaponAfterAction weaponAfterAction;
    Weapon curWeapon => weaponAdvanceUser.currentWeapon;
    public AimDownSightWeaponManuverNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        weaponAfterAction = weaponAdvanceUser.weaponAfterAction;
    }

    public override void Enter()
    {
        weaponAfterAction.AimDownSight(curWeapon);
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdateNode()
    {

        if (weaponManuverManager == null)
            Debug.Log(weaponManuverManager + "is null");

        weaponManuverManager.aimingWeight = Mathf.Clamp01(weaponManuverManager.aimingWeight + Time.deltaTime * curWeapon.aimDownSight_speed);
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
        if(weaponManuverManager == null)
            weaponManuverManager = weaponAdvanceUser.weaponManuverManager;

        weaponAfterAction.AimDownSight(curWeapon);

        if(weaponManuverManager.aimingWeight >= 1)
        {
            Debug.Log("Enemy IspullTriggerManuver =" + weaponManuverManager.isPullTriggerManuver);
            weaponManuverManager.WeaponCommanding();
        }
    }
}
