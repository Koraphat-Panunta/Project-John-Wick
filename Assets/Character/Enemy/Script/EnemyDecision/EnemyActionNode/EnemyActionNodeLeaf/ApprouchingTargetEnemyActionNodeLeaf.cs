using System;
using UnityEngine;

public class ApprouchingTargetEnemyActionNodeLeaf : EnemyActionNodeLeaf
{
    public EnemyMoveCurvePath curvePath;
    private const float MIN_CURVE_MOVE = 5;
    private const float MAX_CURVE_MOVE = 10;
    private const float MIN_APPROUCH_TIME = 6;
    private const float MAX_APPROUCH_TIME = 9;
    private float approuchingTime;
    public ApprouchingTargetEnemyActionNodeLeaf(Enemy enemy, EnemyCommandAPI enemyCommandAPI, Func<bool> preCondition, EnemyActionNodeManager enemyActionNodeManager) : base(enemy, enemyCommandAPI, preCondition, enemyActionNodeManager)
    {
        curvePath = new EnemyMoveCurvePath(MIN_CURVE_MOVE,MAX_CURVE_MOVE);
    }

    public override void Enter()
    {
        approuchingTime = UnityEngine.Random.Range(MIN_APPROUCH_TIME, MAX_APPROUCH_TIME);
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
        if(approuchingTime <= 0)
            return true;

        return base.IsComplete();
    }

    public override bool IsReset()
    {
        if(enemy._currentWeapon == null)
            return true;

        return IsComplete();
    }

    public override void UpdateNode()
    {
        approuchingTime -= Time.deltaTime;

        switch (enemyActionNodeManager.curCombatPhase)
        {
            case IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert:
                {
                    enemyCommandAPI.AimDownSight(enemy.targetKnewPos);
                    enemyCommandAPI.NormalFiringPattern.Performing();

                }
                break;
            case IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware:
                {
                    enemyCommandAPI.AimDownSight(enemy.targetKnewPos);
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

        Vector3 endPoint = enemy.targetKnewPos + ((enemy.targetKnewPos - enemy.transform.position).normalized * 4);

        curvePath.AutoRegenaratePath(endPoint, enemy.transform.position, 2);

        if (curvePath._curvePoint.Count > 0)
            if (enemyCommandAPI.MoveToPosition(curvePath._curvePoint.Peek(), 1, 2.25f))
            {
                enemyCommandAPI.FreezPosition();
                curvePath._curvePoint.Dequeue();
            }
    }
}
