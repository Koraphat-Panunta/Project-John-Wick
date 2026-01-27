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
        
        (this.weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon)._weaponAttacherComponent.SetOffsetPosition(this.quickSwitchHoldOffset.postitionOffset);
        (this.weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon)._weaponAttacherComponent.SetOffserRotation(Quaternion.Euler(this.quickSwitchHoldOffset.rotationEulerOffset));

        base.UpdateNode();
    }
    public override void Exit()
    {
        if (quickSwitchWeaponManuverAble.isQuickSwtichWeaponManuverAble == false)
            WeaponAttachingBehavior.Attach(weaponAdvanceUser._secondHandSocket.curWeaponAtSocket,weaponAdvanceUser._weaponBelt.primaryWeaponSocket, WeaponMountComponent.attatchingDurationGlobal);
        base.Exit();
    }
   


}
