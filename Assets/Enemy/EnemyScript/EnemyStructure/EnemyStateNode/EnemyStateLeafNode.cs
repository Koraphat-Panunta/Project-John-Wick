using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyStateLeafNode : EnemyStateNode, INodeLeaf
{
    public List<Func<bool>> isReset { get; set ; }
    public NodeLeafBehavior nodeLeafBehavior { get; set ; }
    public bool isComplete { get; protected set ; }

    public EnemyStateLeafNode(Enemy enemy, Func<bool> preCondition) : base(enemy, preCondition)
    {
        isReset = new List<Func<bool>>();
        nodeLeafBehavior = new NodeLeafBehavior();
    }

    public virtual void Enter()
    {
        isComplete = false;
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
        Debug.Log("Enemy Is Reset Count = " + isReset.Count);

        return nodeLeafBehavior.IsReset(isReset);
    } 
   
    public virtual void UpdateNode()
    {
       
    }
}
