using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponManuverLeafNode : WeaponManuverNode
{
    public List<Func<bool>> isReset { get; protected set; }
    public WeaponManuverSelectorNode parentNode;
    public WeaponManuverLeafNode(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        isReset = new List<Func<bool>>();
        isReset.Add(() => !preCondition());
    }
    public abstract void UpdateNode();
    public abstract void Enter();
    public abstract void Exit();

    public abstract void FixedUpdateNode();
    public abstract bool IsComplete();
    public override bool Precondition()
    {
        return base.Precondition();
    }
    public virtual bool IsReset()
    {
        foreach(Func<bool> reset in isReset)
        {
            if(reset.Invoke() == true)
                return true;
        }

        return false;
    }
}
