using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class EnemyActionNode 
{
    public EnemyActionNode(EnemyControllerAPI enemyController)
    {
        this.enemyController = enemyController;
        this.enemy = enemyController.enemy;
        childNode = new List<EnemyActionNode>();
    }

    protected Enemy enemy { get; set; }
    protected EnemyControllerAPI enemyController { get; set; }
    public abstract List<EnemyActionNode> childNode { get; set; }
    protected abstract Func<bool> preCondidtion { get; set; }
    protected abstract Func<float> getCost { get; set; }
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract bool IsReset();
    public abstract bool PreCondition();
    public virtual float GetCost()
    {
        return getCost.Invoke();
    }
    public void Transition(out EnemyActionLeafNode enemyActionLeaf)
    {
        enemyActionLeaf = null;
        foreach (EnemyActionNode eAction in childNode)
        {
            if (eAction.PreCondition() == false)
            { continue; }

            if (eAction.GetType().IsSubclassOf(typeof(EnemyActionLeafNode)))
            {
                enemyActionLeaf = eAction as EnemyActionLeafNode;
                Debug.Log("Transition from " + this + " ->" + eAction);
            }
            else
            {
                Debug.Log("Transition from " + this + " ->" + eAction);
                eAction.Transition(out enemyActionLeaf);
            }
            break;
        }
    }
    public void AddChildNode(EnemyActionNode enemyActionNode)
    {
        childNode.Add(enemyActionNode);
    }
}
