using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponManuverLeafNode : WeaponManuverNode,INodeLeaf
{
    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set; }

    public enum WeaponManuverLeafNodePhase
    {
        Enter,
        Update,
        Exit
    }

    public WeaponManuverLeafNodePhase curPhase { get; protected set; }

    public WeaponManuverLeafNode(IRangeWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        nodeLeafBehavior = new NodeLeafBehavior();

        isReset = new List<Func<bool>>();
        isReset.Add(() => !preCondition());
    }

    public abstract void UpdateNode();

    public abstract void FixedUpdateNode();
  
    public virtual void Enter()
    {
        this.curPhase = WeaponManuverLeafNodePhase.Enter;
    }


    public virtual void Exit()
    {
        this.curPhase = WeaponManuverLeafNodePhase.Exit;
    }

    public abstract bool IsComplete();

    public virtual bool IsReset() => nodeLeafBehavior.IsReset(isReset);
   
}
