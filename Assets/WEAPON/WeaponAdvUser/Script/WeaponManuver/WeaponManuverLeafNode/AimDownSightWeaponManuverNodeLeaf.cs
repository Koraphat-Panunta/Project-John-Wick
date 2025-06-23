using System;
using UnityEngine;

public class AimDownSightWeaponManuverNodeLeaf : WeaponManuverLeafNode
{
    WeaponManuverManager weaponManuverManager => weaponAdvanceUser._weaponManuverManager;
    WeaponAfterAction weaponAfterAction;
    Weapon curWeapon => weaponAdvanceUser._currentWeapon;
    public enum AimDownSightPhase
    {
        Enter,
        Update,
        Exit
    }
    public AimDownSightPhase curPhase { get; protected set; }
    public AimDownSightWeaponManuverNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        weaponAfterAction = weaponAdvanceUser._weaponAfterAction;
    }

    public override void Enter()
    {
        curPhase = AimDownSightPhase.Enter;
        weaponAfterAction.SendFeedBackWeaponAfterAction
            <AimDownSightWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive,this);
    }

    public override void Exit()
    {
        curPhase = AimDownSightPhase.Exit;
        weaponAfterAction.SendFeedBackWeaponAfterAction
            <AimDownSightWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }

    public override void FixedUpdateNode()
    {

        if (weaponManuverManager == null)
            Debug.Log(weaponManuverManager + "is null");

        weaponManuverManager.aimingWeight = Mathf.Clamp01(weaponManuverManager.aimingWeight + Time.deltaTime * curWeapon.aimDownSight_speed);

        curPhase = AimDownSightPhase.Update;
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
        //weaponAfterAction.SendFeedBackWeaponAfterAction
        //    <AimDownSightWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);

        if (weaponManuverManager.isPullTriggerManuverAble && weaponAdvanceUser._isPullTriggerCommand)
            curWeapon.PullTrigger();
    }
}
