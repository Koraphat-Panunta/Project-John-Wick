using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class EnemyStateNode 
{
    public EnemyStateNode(Enemy enemy)
    {
        this.enemy = enemy;
        childNode = new List<EnemyStateNode>();
    }

    protected Enemy enemy { get; set; }
    public abstract List<EnemyStateNode> childNode { get; set; }
    protected abstract Func<bool> preCondidtion { get; set; }
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract bool IsReset();
    public abstract bool PreCondition();
    public void Transition(out EnemyStateLeafNode enemyStateActionNode)
    {
        enemyStateActionNode = null;
        foreach (EnemyStateNode stateNode in childNode)
        {
            if (stateNode.PreCondition())
            {
                if (stateNode.GetType().IsSubclassOf(typeof(EnemyStateLeafNode)))
                {
                    enemyStateActionNode = stateNode as EnemyStateLeafNode;
                    //Debug.Log("Transition from " + this + " ->" + weaponActionNode);
                }
                else
                {
                    //Debug.Log("Transition from " + this + " ->" + weaponNode);
                    stateNode.Transition(out enemyStateActionNode);
                }
                break;
            }
        }
    }
    public void AddChildNode(EnemyStateNode stateNode)
    {
        childNode.Add(stateNode);
    }



}
