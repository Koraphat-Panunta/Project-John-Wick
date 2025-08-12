using System;
using UnityEngine;

public class QuickSwitch_LowReady_NodeLeaf : LowReadyWeaponManuverNodeLeaf, IQuickSwitchNode
{
    public QuickSwitch_LowReady_NodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
    }
}
