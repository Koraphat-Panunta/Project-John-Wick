using System;
using UnityEngine;

public class TimeControlNode : INode
{
    public Func<bool> preCondition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public INode parentNode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public TimeControlNode(Func<bool> preCondition) 
    {
        this.preCondition = preCondition;
    }
    public bool Precondition() => preCondition.Invoke();
   
}
