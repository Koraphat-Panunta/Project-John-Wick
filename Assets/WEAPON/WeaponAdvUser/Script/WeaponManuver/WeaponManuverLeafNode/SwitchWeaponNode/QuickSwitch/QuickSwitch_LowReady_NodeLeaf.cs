using System;
using UnityEngine;

public class QuickSwitch_LowReady_NodeLeaf : LowReadyWeaponManuverNodeLeaf, IQuickSwitchNode
{
    public IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble { get; set; }
    public QuickSwitch_LowReady_NodeLeaf(IWeaponAdvanceUser weaponAdvanceUser,IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        this.quickSwitchWeaponManuverAble = quickSwitchWeaponManuverAble;
    }
    public override void Exit()
    {
        if (quickSwitchWeaponManuverAble.isQuickSwtichWeaponManuverAble == false)
            WeaponAttachingBehavior.Attach(weaponAdvanceUser._secondHandSocket.curWeaponAtSocket,weaponAdvanceUser._weaponBelt.primaryWeaponSocket);
        base.Exit();
    }
   


}
