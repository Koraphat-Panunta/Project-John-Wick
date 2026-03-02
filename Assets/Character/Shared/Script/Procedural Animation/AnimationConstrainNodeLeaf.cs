using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationConstrainNodeLeaf : AnimationConstrainNode, INodeLeaf
{
    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set; }
    protected bool isComplete;
    public AnimationConstrainNodeLeaf(Func<bool> precondition) : base(precondition)
    {
        isReset = new List<Func<bool>>();
        nodeLeafBehavior = new NodeLeafBehavior();
    }
    public virtual void Enter()
    {
        this.isTriggerReset = false;
    }

    public virtual void Exit()
    {
        this.isTriggerReset = false;
    }

    public virtual void FixedUpdateNode()
    {
       
    }

    public virtual bool IsComplete()
    {
        return isComplete;
    }

    protected bool isTriggerReset;
    public void TriggerReset()
    {
        this.isTriggerReset = true;
    }
    public virtual bool IsReset() 
    {
        if (this.isTriggerReset)
            return true;

        return this.nodeLeafBehavior.IsReset(isReset);
    } 
    
   

    public virtual void UpdateNode()
    {
        
    }
}
