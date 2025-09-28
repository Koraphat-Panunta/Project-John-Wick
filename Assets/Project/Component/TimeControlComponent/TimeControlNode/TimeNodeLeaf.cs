using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeNodeLeaf : TimeControlNode, INodeLeaf
{
    protected TimeControlManager timeControlManager { get; set; }
    public TimeNodeLeaf(Func<bool> preCondition,TimeControlManager timeControlManager) : base(preCondition)
    {
        this.isReset = new List<Func<bool>>();
        this.nodeLeafBehavior = new NodeLeafBehavior();
        this.timeControlManager = timeControlManager;
    }
    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set; }
    public enum TimeNodeLeafPhase
    {
        Enter,
        Exit
    }
    public TimeNodeLeafPhase curPhase { get;protected set; }
    protected bool isComplete;

    public virtual void Enter()
    {
        isComplete = false;
        curPhase = TimeNodeLeafPhase.Enter;
        this.timeControlManager.NotifyObserver(this.timeControlManager, this);
    }

    public virtual void Exit()
    {
        curPhase = TimeNodeLeafPhase.Exit;
        this.timeControlManager.NotifyObserver(this.timeControlManager, this);
    }
    public abstract void UpdateNode();

    public abstract void FixedUpdateNode(); 

    public bool IsComplete() => isComplete;
  

    public virtual bool IsReset() => nodeLeafBehavior.IsReset(isReset);
   
}
