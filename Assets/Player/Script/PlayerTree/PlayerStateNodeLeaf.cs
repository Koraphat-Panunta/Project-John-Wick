using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateNodeLeaf : PlayerStateNode, INodeLeaf
{
    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set; }
    public bool isComplete { get; protected set; }

    public PlayerStateNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
        isReset = new List<Func<bool>>();
        nodeLeafBehavior = new NodeLeafBehavior();
    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void FixedUpdateNode()
    {

    }

    public virtual bool IsComplete()
    {
        return isComplete;
    }

    public virtual bool IsReset()
    {
        Debug.Log("Player Is Reset Count = " + isReset.Count);

        return nodeLeafBehavior.IsReset(isReset);
    }


    public virtual void UpdateNode()
    {

    }
}
