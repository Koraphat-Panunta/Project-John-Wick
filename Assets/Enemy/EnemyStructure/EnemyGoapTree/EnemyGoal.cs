using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class EnemyGoal 
{
    public EnemyGoal(Enemy enemy)
    {
        this.enemy = enemy;
        childNode = new List<EnemyGoal>();
    }

    protected Enemy enemy { get; set; }
    public abstract List<EnemyGoal> childNode { get; set; }
    protected abstract Func<bool> preCondidtion { get; set; }
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract bool IsReset();
    public abstract bool PreCondition();
    public void Transition(out EnemyGoalLeaf enemyGoalLeaf)
    {
        enemyGoalLeaf = null;
        foreach (EnemyGoal eGoal in childNode)
        {
            if (eGoal.PreCondition() == false)
            { continue; }

            if (eGoal.GetType().IsSubclassOf(typeof(EnemyGoalLeaf)))
            {
                enemyGoalLeaf = eGoal as EnemyGoalLeaf;
                Debug.Log("Transition from " + this + " ->" + eGoal);
            }
            else
            {
                Debug.Log("Transition from " + this + " ->" + eGoal);
                eGoal.Transition(out enemyGoalLeaf);
            }
            break;
        }
    }
    public void AddChildNode(EnemyGoal enemyGoal)
    {
        childNode.Add(enemyGoal);
    }
}
