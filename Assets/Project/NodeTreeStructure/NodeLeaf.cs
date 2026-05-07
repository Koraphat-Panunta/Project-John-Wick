using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeLeaf : INodeLeaf
{
    public List<Func<bool>> isReset { get; set ; }
    public NodeLeafBehavior nodeLeafBehavior { get ; set ; }
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }

    protected bool isComplete;

    public NodeLeaf(Func<bool> preCondition)
    {
        this.isReset = new List<Func<bool>>();
        this.nodeLeafBehavior = new NodeLeafBehavior();
        this.preCondition = preCondition;
    }

    public virtual void Enter()
    {
        this.isComplete = false;
    }

    public virtual void Exit()
    {
        
    }

    public virtual void FixedUpdateNode()
    {
        
    }

    public virtual bool IsComplete()
    {
        return this.isComplete;
    }

    public virtual bool IsReset()
    {
        return this.nodeLeafBehavior.IsReset(this.isReset);
    }

    public virtual bool Precondition()
    {
        return this.preCondition.Invoke();
    }

    public virtual void UpdateNode()
    {
        
    }
}
