using System.Collections.Generic;
using System;
using UnityEngine;

public interface INodeLeaf : INode
{
    public List<Func<bool>> isReset { get;  set; }
    public NodeLeafBehavior nodeLeafBehavior { get;  set; }
    public abstract void UpdateNode();
    public abstract void Enter();
    public abstract void Exit();
    public abstract void FixedUpdateNode();
    public abstract bool IsComplete();
    public bool IsReset() ;

}
public class NodeLeafBehavior
{
    public bool IsReset(List<Func<bool>> isReset)
    {
        foreach (Func<bool> reset in isReset)
        {
            if (reset.Invoke() == true)
                return true;
        }

        return false;
    }
}

