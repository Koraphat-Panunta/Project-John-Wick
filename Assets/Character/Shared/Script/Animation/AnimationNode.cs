using System;
using UnityEngine;

public class AnimationNode : INode
{
    public Func<bool> preCondition { get ; set ; }
    public INode parentNode { get ; set ; }

    public AnimationNode(Func<bool> preCondition)
    {
        this.preCondition = preCondition;
    }

    public virtual bool Precondition()
    {
        return preCondition.Invoke();
    }
}
