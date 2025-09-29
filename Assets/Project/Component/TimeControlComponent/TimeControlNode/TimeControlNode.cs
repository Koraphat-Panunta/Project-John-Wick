using System;
using UnityEngine;

public class TimeControlNode : INode
{
    public Func<bool> preCondition { get ; set; }
    public INode parentNode { get; set ; }
    public TimeControlNode(Func<bool> preCondition) 
    {
        this.preCondition = preCondition;
    }
    public bool Precondition() => preCondition.Invoke();
   
}
