using System.Collections.Generic;
using System;
using UnityEngine;
using static EnemyStateLeafNode;

public class EnemyStateLeafNode : EnemyStateNode, INodeLeaf
{
    public List<Func<bool>> isReset { get; set ; }
    public NodeLeafBehavior nodeLeafBehavior { get; set ; }
    public virtual bool isComplete { get; protected set ; }
    public enum Curstate
    {
        Enter,
        Exit,   
    }
    public Curstate curstate { get; protected set ; }

    public EnemyStateLeafNode(Enemy enemy, Func<bool> preCondition) : base(enemy, preCondition)
    {
        isReset = new List<Func<bool>>();
        nodeLeafBehavior = new NodeLeafBehavior();
    }

    public virtual void Enter()
    {
        isComplete = false;
        curstate = Curstate.Enter;
        enemy.NotifyObserver(enemy, this);
    }

    public virtual void Exit()
    {
        curstate = Curstate.Exit;
        enemy.NotifyObserver(enemy, this);
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

        return nodeLeafBehavior.IsReset(isReset);
    } 
   
    public virtual void UpdateNode()
    {
       
    }
}
