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
    protected abstract Func<float> getCost { get; set; }
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract bool IsReset();
    public abstract bool PreCondition();
    public abstract float GetCost();
    public void Transition(out EnemyGoalLeaf enemyGoalLeaf)
    {
        enemyGoalLeaf = null;
        EnemyGoal curGoal = null;
        foreach (EnemyGoal eGoal in childNode)
        {
            if (eGoal.PreCondition() == false)
            { continue; }

            if (curGoal == null){
                curGoal = eGoal;
                continue;
            }

            if(eGoal.GetCost()>curGoal.GetCost())
                { curGoal = eGoal; }
        }

        if (curGoal.GetType().IsSubclassOf(typeof(EnemyGoalLeaf)))
        {
           enemyGoalLeaf = curGoal as EnemyGoalLeaf;    
        }
        else
        {
            curGoal.Transition(out EnemyGoalLeaf enemyGoal);
            enemyGoalLeaf = enemyGoal;
        }
    }
    public void AddChildNode(EnemyGoal enemyGoal)
    {
        childNode.Add(enemyGoal);
    }
}
