using System;
using UnityEngine;

public class ApprouchingTargetEnemyActionNodeLeaf : EnemyActionNodeLeaf
{
    private CurvePath curvePath;
    public ApprouchingTargetEnemyActionNodeLeaf(Enemy enemy, EnemyCommandAPI enemyCommandAPI, Func<bool> preCondition, EnemyActionNodeManager enemyActionNodeManager) : base(enemy, enemyCommandAPI, preCondition, enemyActionNodeManager)
    {
        curvePath = new CurvePath();
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
        switch (enemyActionNodeManager.curCombatPhase)
        {
            case EnemyDecision.CombatPhase.Alert:
                {
                    enemyCommandAPI.AimDownSight(enemy.targetKnewPos, 5);
                    enemyCommandAPI.NormalFiringPattern.Performing();

                }
                break;
            case EnemyDecision.CombatPhase.Aware:
                {
                    enemyCommandAPI.AimDownSight(enemy.targetKnewPos, 5);
                }
                break;
        }

        curvePath.AutoRegenaratePath(enemy.targetKnewPos, enemy.transform.position, 2);

        if (curvePath._curvePoint.Count > 0)
            if (enemyCommandAPI.MoveToPosition(curvePath._curvePoint.Peek(), 1))
            {
                enemyCommandAPI.Freez();
                curvePath._curvePoint.Dequeue();
            }

        base.UpdateNode();
    }
}
