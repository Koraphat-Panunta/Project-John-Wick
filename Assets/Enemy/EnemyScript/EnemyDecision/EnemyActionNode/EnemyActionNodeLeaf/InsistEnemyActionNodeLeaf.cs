using System;
using UnityEngine;

public class InsistEnemyActionNodeLeaf : EnemyActionNodeLeaf
{
    public InsistEnemyActionNodeLeaf(Enemy enemy, EnemyCommandAPI enemyCommandAPI, Func<bool> preCondition, EnemyActionNodeManager enemyActionNodeManager) 
        : base(enemy, enemyCommandAPI, preCondition, enemyActionNodeManager)
    {

    }

    public override void Enter()
    {
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
        switch (enemyActionNodeManager.enemyDecision._curCombatPhase)
        {
            case IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert:
                {
                    enemyCommandAPI.Freez();
                    enemyCommandAPI.AimDownSight(enemy.targetKnewPos, 5);
                    enemyCommandAPI.NormalFiringPattern.Performing();

                }
                break;
            case IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware:
                {
                    enemyCommandAPI.Freez();
                    enemyCommandAPI.AimDownSight(enemy.targetKnewPos, 5);

                }
                break;
        }
        base.UpdateNode();
    }
}
