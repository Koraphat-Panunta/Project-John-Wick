using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class InWorldUINodeLeaf : InWorldUINode, INodeLeaf
{

    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set; }
    protected bool isComplete { get; set; }
    public InWorldUINodeLeaf(Func<bool> preCondition) : base(preCondition)
    {
        isReset = new List<Func<bool>>();
        nodeLeafBehavior = new NodeLeafBehavior();
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {
       
    }

    public virtual void FixedUpdateNode()
    {
       
    }

    public virtual bool IsComplete()
    {
        return isComplete;
    }

    public virtual bool IsReset()
    {
        return nodeLeafBehavior.IsReset(isReset);
    }

    public virtual void UpdateNode()
    {
        
    }
}
