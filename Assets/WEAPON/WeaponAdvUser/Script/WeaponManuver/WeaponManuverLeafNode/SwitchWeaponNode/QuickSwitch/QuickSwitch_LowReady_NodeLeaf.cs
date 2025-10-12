using System;
using UnityEngine;

public class QuickSwitch_LowReady_NodeLeaf : LowReadyWeaponManuverNodeLeaf, IQuickSwitchNode
{
    public IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble { get; set; }
    private TransformOffsetSCRP quickSwitchHoldOffset;
    public QuickSwitch_LowReady_NodeLeaf(IWeaponAdvanceUser weaponAdvanceUser,IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble,TransformOffsetSCRP quickSwitchOffset, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        this.quickSwitchWeaponManuverAble = quickSwitchWeaponManuverAble;
        this.quickSwitchHoldOffset = quickSwitchOffset;
    }
    public override void UpdateNode()
    {
        WeaponAttachingBehavior.Attach(
           weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon
           , weaponAdvanceUser._secondHandSocket
           , quickSwitchHoldOffset.postitionOffset
           , Quaternion.Euler(quickSwitchHoldOffset.rotationEulerOffset));

        base.UpdateNode();
    }
    public override void Exit()
    {
        if (quickSwitchWeaponManuverAble.isQuickSwtichWeaponManuverAble == false)
            WeaponAttachingBehavior.Attach(weaponAdvanceUser._secondHandSocket.curWeaponAtSocket,weaponAdvanceUser._weaponBelt.primaryWeaponSocket);
        base.Exit();
    }
   


}
