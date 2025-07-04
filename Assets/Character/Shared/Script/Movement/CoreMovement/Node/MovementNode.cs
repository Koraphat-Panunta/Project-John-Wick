using System;
using UnityEngine;

public abstract class MovementNode : INode
{
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }
    public MovementNode(Func<bool> preCondition)
    {
        this.preCondition = preCondition;
    }
    public virtual bool Precondition()
    {
       return preCondition.Invoke();
    }
}
