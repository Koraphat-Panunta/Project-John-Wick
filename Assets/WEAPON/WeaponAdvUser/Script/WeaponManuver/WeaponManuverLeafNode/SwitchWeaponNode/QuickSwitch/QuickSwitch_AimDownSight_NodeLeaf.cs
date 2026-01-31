using System;
using UnityEngine;

public class QuickSwitch_AimDownSight_NodeLeaf : AimDownSightWeaponManuverNodeLeaf, IQuickSwitchNode
{
    public IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble { get; set; }
    public QuickSwitch_AimDownSight_NodeLeaf(IWeaponAdvanceUser weaponAdvanceUser,IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        this.quickSwitchWeaponManuverAble = quickSwitchWeaponManuverAble;
    }

    protected override void AimingWeightUpdate()
    {
        if (this.weaponAdvanceUser._isAimingCommand && this.weaponAdvanceUser._weaponManuverManager.isAimingManuverAble)
            this.weaponAdvanceUser._weaponManuverManager.aimingWeight = Mathf.Clamp01(this.weaponAdvanceUser._weaponManuverManager.aimingWeight + Time.deltaTime * this.weaponAdvanceUser._currentWeapon.aimDownSight_speed);
        else
            this.weaponAdvanceUser._weaponManuverManager.aimingWeight = Mathf.Clamp01(this.weaponAdvanceUser._weaponManuverManager.aimingWeight - Time.deltaTime * this.weaponAdvanceUser._currentWeapon.aimDownSight_speed);

    }

    public override void Exit()
    {
        if (quickSwitchWeaponManuverAble.isQuickSwtichWeaponManuverAble == false)
            WeaponAttachingBehavior.Attach(weaponAdvanceUser._secondHandSocket.curWeaponAtSocket, weaponAdvanceUser._weaponBelt.primaryWeaponSocket, WeaponMountComponent.attatchingDurationGlobal);
        base.Exit();
    }

}
