using System.Collections.Generic;
using System;
using UnityEngine;

public class MovementNodeLeaf : MovementNode,INodeLeaf
{
    public MovementNodeLeaf(Func<bool> preCondition) : base(preCondition)
    {
        nodeLeafBehavior = new NodeLeafBehavior();
    }

    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set; }

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
        return true;
    }

    public virtual bool IsReset()
    {
        return nodeLeafBehavior.IsReset(isReset);
    }

    public virtual void UpdateNode()
    {
    }
}
