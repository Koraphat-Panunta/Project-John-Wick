using System;
using UnityEngine;

public class QuickSwitch_Reload_NodeLeaf : WeaponManuverLeafNode, IQuickSwitchNode
{

    public IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble { get; set; }
    public QuickSwitch_Reload_NodeLeaf(IRangeWeaponAdvanceUser weaponAdvanceUser,IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        this.quickSwitchWeaponManuverAble = quickSwitchWeaponManuverAble;
    }
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        weaponAdvanceUser._isReloadCommand = true;
    }

    public override void FixedUpdateNode()
    {
        
    }

    public override bool IsComplete()
    {
        return true;
    }

    public override void UpdateNode()
    {
        if (this.weaponAdvanceUser._isAimingCommand && this.weaponAdvanceUser._weaponManuverManager.isAimingManuverAble)
            this.weaponAdvanceUser._weaponManuverManager.aimingWeight = Mathf.Clamp01(this.weaponAdvanceUser._weaponManuverManager.aimingWeight + Time.deltaTime * IQuickSwitchNode.adsSpeed);
        else
            this.weaponAdvanceUser._weaponManuverManager.aimingWeight = Mathf.Clamp01(this.weaponAdvanceUser._weaponManuverManager.aimingWeight - Time.deltaTime * IQuickSwitchNode.adsSpeed);
    }
}
