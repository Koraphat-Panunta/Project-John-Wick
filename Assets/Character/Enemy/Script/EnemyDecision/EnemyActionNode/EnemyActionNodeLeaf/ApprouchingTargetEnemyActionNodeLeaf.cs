using System;
using UnityEngine;

public class ApprouchingTargetEnemyActionNodeLeaf : EnemyActionNodeLeaf
{
    public EnemyMoveCurvePath curvePath;
    private const float MIN_CURVE_MOVE = 5;
    private const float MAX_CURVE_MOVE = 10;    
    public ApprouchingTargetEnemyActionNodeLeaf(Enemy enemy, EnemyCommandAPI enemyCommandAPI, Func<bool> preCondition, EnemyActionNodeManager enemyActionNodeManager) : base(enemy, enemyCommandAPI, preCondition, enemyActionNodeManager)
    {
        curvePath = new EnemyMoveCurvePath(MIN_CURVE_MOVE,MAX_CURVE_MOVE);
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
            case IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert:
                {
                    enemyCommandAPI.AimDownSight(enemy.targetKnewPos, 5);
                    enemyCommandAPI.NormalFiringPattern.Performing();

                }
                break;
            case IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware:
                {
                    enemyCommandAPI.AimDownSight(enemy.targetKnewPos, 5);
                }
                break;
        }

        MovementDecisionUpdate();
        base.UpdateNode();
    }
    private void MovementDecisionUpdate()
    {
        if(Vector3.Distance(enemy.transform.position,enemy.targetKnewPos) <= 2f)
            return;

        curvePath.AutoRegenaratePath(enemy.targetKnewPos, enemy.transform.position, 2);

        if (curvePath._curvePoint.Count > 0)
            if (enemyCommandAPI.MoveToPosition(curvePath._curvePoint.Peek(), 1, 2.25f))
            {
                enemyCommandAPI.FreezPosition();
                curvePath._curvePoint.Dequeue();
            }
    }
}
