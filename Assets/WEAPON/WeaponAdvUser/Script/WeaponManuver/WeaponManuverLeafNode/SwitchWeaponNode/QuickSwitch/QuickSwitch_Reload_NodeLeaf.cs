using System;
using UnityEngine;

public class QuickSwitch_Reload_NodeLeaf : WeaponManuverLeafNode, IQuickSwitchNode
{
    public QuickSwitch_Reload_NodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
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
        
    }
}
