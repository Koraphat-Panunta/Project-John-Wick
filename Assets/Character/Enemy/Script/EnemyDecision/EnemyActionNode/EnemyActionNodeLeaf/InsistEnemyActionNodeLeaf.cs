using System;
using Unity.Mathematics;
using UnityEngine;

public class InsistEnemyActionNodeLeaf : EnemyActionNodeLeaf
{
    private Vector3 insistPos;
    private float distance;
    protected IEnemyActionNodeManagerImplementDecision enemyActionNodeManagerImplementDecision;
    public InsistEnemyActionNodeLeaf(Enemy enemy
        , EnemyCommandAPI enemyCommandAPI
        , Func<bool> preCondition
        , EnemyDecision enemyDecision
        ,IEnemyActionNodeManagerImplementDecision enemyActionNodeManagerImplementDecision) 
        : base(enemy, enemyCommandAPI, preCondition, enemyDecision)
    {
        this.enemyActionNodeManagerImplementDecision = enemyActionNodeManagerImplementDecision;
    }

    public override void Enter()
    {
        insistPos = enemy.transform.position;
        distance = UnityEngine.Random.Range(1, 3);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return base.IsComplete();
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override void UpdateNode()
    {
        switch (this.enemyActionNodeManagerImplementDecision._curCombatPhase)
        {
            case IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert:
                {
                    enemyCommandAPI.FreezPosition();
                    enemyCommandAPI.AimDownSight(enemy.targetKnewPos);
                    enemyCommandAPI.NormalFiringPattern.Performing();
                    enemyCommandAPI.enemyAutoDefendCommand.UpdateAutoDefend();

                    //if (Vector3.Distance(insistPos, enemy.transform.position) < distance 
                    //    && Vector3.Distance(enemy.transform.position, enemy.targetKnewPos) > f)
                    //    enemyCommandAPI.MoveToPosition(enemy.targetKnewPos, enemy.moveMaxSpeed);
                    //else
                    //    enemyCommandAPI.FreezPosition();

                }
                break;
            case IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware:
                {
                    enemyCommandAPI.FreezPosition();
                    enemyCommandAPI.AimDownSight(enemy.targetKnewPos);
                    enemyCommandAPI.enemyAutoDefendCommand.UpdateAutoDefend();

                }
                break;
            case IEnemyActionNodeManagerImplementDecision.CombatPhase.Chill:
                {
                    enemyCommandAPI.FreezPosition();
                    enemyCommandAPI.LowReady();
                }
                break;
        }
        base.UpdateNode();
    }
}
