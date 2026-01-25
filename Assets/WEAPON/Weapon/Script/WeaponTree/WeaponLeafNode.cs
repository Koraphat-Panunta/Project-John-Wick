using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponLeafNode : WeaponNode, INodeLeaf
{
    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get ; set ; }
    public bool isComplete { get; protected set; }

    public enum WeaponNodeLeafPhase
    {
        Enter,
        Exit
    }

    public WeaponNodeLeafPhase curWeaponNodeLeafPhase { get; protected set; }

    protected WeaponLeafNode(Weapon weapon, Func<bool> preCondition) : base(weapon, preCondition)
    {
        this.isReset = new List<Func<bool>>();
        this.nodeLeafBehavior = new NodeLeafBehavior();
    }

    public virtual void Enter()
    {
        curWeaponNodeLeafPhase = WeaponNodeLeafPhase.Enter;
        base.Weapon.Notify(base.Weapon, this);
    }

    public virtual void Exit()
    {
        curWeaponNodeLeafPhase = WeaponNodeLeafPhase.Exit;
        base.Weapon.Notify(base.Weapon, this);
    }

    public abstract void FixedUpdateNode();
  
    public virtual bool IsComplete()
    {
        return isComplete;
    }
    public virtual bool IsReset() => nodeLeafBehavior.IsReset(isReset);
    public abstract void UpdateNode();
   
}
