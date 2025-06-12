using System;
using UnityEngine;

public abstract class AnimationConstrainNode : INode
{
    public Func<bool> preCondition { get; set ; }
    public INode parentNode { get; set; }

    public AnimationConstrainNode(Func<bool> precondition)
    {
        this.preCondition = precondition;
    }
    public bool Precondition() => preCondition.Invoke();
    
}
