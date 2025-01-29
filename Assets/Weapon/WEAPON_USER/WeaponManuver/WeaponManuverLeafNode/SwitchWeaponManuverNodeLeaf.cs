using System;
using UnityEngine;

public class SwitchWeaponManuverNodeLeaf : WeaponManuverLeafNode
{
    public enum SwitchingManuver
    {
        PrimaryToSecondary,
        SecondaryToPrimary,
        None
    }
    private SwitchingManuver curSwitchManuver;
    private bool isComplete;
    public SwitchWeaponManuverNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        curSwitchManuver = SwitchingManuver.None;
    }

    public override void FixedUpdateNode()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsComplete()
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }

    public override void Enter()
    {
        throw new NotImplementedException();
    }

    public override void Exit()
    {
        throw new NotImplementedException();
    }
}
