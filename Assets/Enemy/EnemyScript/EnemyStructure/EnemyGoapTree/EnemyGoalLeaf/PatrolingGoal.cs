using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolingGoal : EnemyGoalLeaf
{
    public List<PatrolPoint> patrolPoints;
    public PatrolPoint myPatrolPoint;
    public bool isMovingToPatrol;
    public PatrolingGoal(EnemyCommandAPI enemyController, IEnemyGOAP enemyGOAP,IPatrolComponent patroler) : base(enemyController, enemyGOAP)
    {
        patrolPoints = patroler.patrolPoints;
        isMovingToPatrol = true;
        InitailizedActionNode();
    }
    public override List<EnemyGoal> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }


   

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if(enemy.combatOffensiveInstinct.myCombatPhase != CombatOffensiveInstinct.CombatPhase.Chill)
            return true;

        if(enemy.findingTargetComponent.isSpottingTarget)
            return true;

        return false;
    }

    public override bool PreCondition()
    {
        return true;
    }

    public override void Update()
    {
        if (enemy.findingTargetComponent.FindTarget(out GameObject target))
            enemy.targetKnewPos = target.transform.position;

        base.Update();
    }

    protected override EnemyActionLeafNode enemyActionLeaf { get; set; }
    protected override EnemyActionSelectorNode startActionSelector { get; set; }

    private MoveToPatrolPosition moveToPatrolPosition { get; set; }
    private Wait wait { get; set; }
    protected override void InitailizedActionNode()
    {
        startActionSelector = new EnemyActionSelectorNode(enemyController,()=>true);

        moveToPatrolPosition = new MoveToPatrolPosition(enemyController,enemy,
            () => 
            {
                if(isMovingToPatrol == true)
                    return true;

                return false;
            }, //Precondition
            () => 
            {
                if (Vector3.Distance(enemy.transform.position, myPatrolPoint.patrolTrans.position) < 1.5f)
                {
                    isMovingToPatrol = false;
                    return true;
                }

                return false;
            }); //Reset

        wait = new Wait(enemyController, enemy, () => true,
            () =>
            {
                if (wait.waitTime > patrolPoints[enemy.Index].waitTime)
                {
                    isMovingToPatrol = true;
                    return true;
                }

                return false;
            } //Reset
            );

        startActionSelector.AddChildNode(moveToPatrolPosition);
        startActionSelector.AddChildNode(wait);

        startActionSelector.Transition(out EnemyActionLeafNode enemyActionLeaf);
        this.enemyActionLeaf = enemyActionLeaf;
    }
 
}
