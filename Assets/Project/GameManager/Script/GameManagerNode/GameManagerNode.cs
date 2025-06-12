using System;
using UnityEngine;

public abstract class GameManagerNode : INode
{
    public Func<bool> preCondition { get ; set ; }
    public INode parentNode { get ; set ; }



    public GameManagerNode(Func<bool> preCondition)
    {
        this.preCondition = preCondition;

    }

    public virtual bool Precondition() => preCondition.Invoke();
   
}
