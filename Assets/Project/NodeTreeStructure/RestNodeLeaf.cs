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

    public void Enter()
    {

    }

    public void Exit()
    {

    }

    public void FixedUpdateNode()
    {

    }

    public bool IsComplete()
    {
        return true;
    }

    public bool IsReset()
    {
        return this.nodeLeafBehavior.IsReset(this.isReset);
    }

    public bool Precondition()
    {
        return this.preCondition.Invoke();
    }


    public void UpdateNode()
    {

    }
}
