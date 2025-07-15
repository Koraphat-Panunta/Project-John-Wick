using System;
using UnityEngine;

public abstract class InWorldUINode : INode
{

    public InWorldUINode(Func<bool> preCondition)
    {
        this.preCondition = preCondition;
    }
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }

    public bool Precondition()
    {
        return preCondition.Invoke();
    }
}
