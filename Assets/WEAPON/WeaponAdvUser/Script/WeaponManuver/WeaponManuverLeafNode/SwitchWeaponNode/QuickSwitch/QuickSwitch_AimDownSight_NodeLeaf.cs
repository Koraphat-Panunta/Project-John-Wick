using System;
using UnityEngine;

public class QuickSwitch_AimDownSight_NodeLeaf : AimDownSightWeaponManuverNodeLeaf, IQuickSwitchNode
{
    public QuickSwitch_AimDownSight_NodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
    }
}
