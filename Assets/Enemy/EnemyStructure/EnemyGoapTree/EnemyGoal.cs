using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

public abstract class EnemyGoal 
{
    protected IEnemyGOAP enemyGOAP;
    public EnemyGoal(EnemyControllerAPI enemyController, IEnemyGOAP enemyGOAP)
    {
        this.enemyController = enemyController;
        this.enemy = enemyController.enemy;
        childNode = new List<EnemyGoal>();
        this.enemyGOAP = enemyGOAP;
    }
    protected EnemyControllerAPI enemyController { get; set; }
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
                eGoal.Transition(out EnemyGoalLeaf enemyGoal);
                enemyGoalLeaf = enemyGoal;

            }
        }
    }
    public void AddChildNode(EnemyGoal enemyGoal)
    {
        childNode.Add(enemyGoal);
    }
}
