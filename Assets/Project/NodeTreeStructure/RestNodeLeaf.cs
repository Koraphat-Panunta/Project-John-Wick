using System.Collections.Generic;
using System;
using UnityEngine;

public class RestNodeLeaf : INodeLeaf
{
    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set; }
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }
    public RestNodeLeaf(Func<bool> preCondition)
    {
        this.preCondition = preCondition;
        nodeLeafBehavior = new NodeLeafBehavior();
        isReset = new List<Func<bool>>();
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
        return true;
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
