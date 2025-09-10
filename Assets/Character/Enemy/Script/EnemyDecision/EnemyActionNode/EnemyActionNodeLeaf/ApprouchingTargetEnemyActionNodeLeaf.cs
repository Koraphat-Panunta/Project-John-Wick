using System;
using UnityEngine;

public class ApprouchingTargetEnemyActionNodeLeaf : EnemyActionNodeLeaf
{
    private IEnemyActionNodeManagerImplementDecision enemyActionNodeManager;
    public EnemyMoveCurvePath curvePath;
    private const float MIN_CURVE_MOVE = 4;
    private const float MAX_CURVE_MOVE = 8;
    private const float MIN_APPROUCH_TIME = 6;
    private const float MAX_APPROUCH_TIME = 9;
    private float approuchingTime;
    private float approuchCooldOWN;
    public ApprouchingTargetEnemyActionNodeLeaf(Enemy enemy, EnemyCommandAPI enemyCommandAPI, Func<bool> preCondition, EnemyDecision enemyDecision, IEnemyActionNodeManagerImplementDecision enemyActionNodeManager) : base(enemy, enemyCommandAPI, preCondition, enemyDecision)
    {
        curvePath = new EnemyMoveCurvePath(MIN_CURVE_MOVE,MAX_CURVE_MOVE);
        this.enemyActionNodeManager = enemyActionNodeManager;
    }

    public override void Enter()
    {
        Vector3 endPoint = enemy.targetKnewPos + ((enemy.targetKnewPos - enemy.transform.position).normalized * 3);
        curvePath.RegenaratePath(endPoint, enemy.transform.position);
        targetAnchorPos = enemy.targetKnewPos;
        if (enemyCommandAPI.enemyAutoDefendCommand.dodgeCoolDownTimer <= 0)
            enemyCommandAPI.Dodge(curvePath._curvePoint.Peek());

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

        return false;   
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

        switch (enemyActionNodeManager._curCombatPhase)
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
        enemyCommandAPI.enemyAutoDefendCommand.UpdateAutoDefend();
        MovementDecisionUpdate();
        base.UpdateNode();
    }
    private Vector3 targetAnchorPos;
    private void MovementDecisionUpdate()
    {
        if(Vector3.Distance(enemy.transform.position,enemy.targetKnewPos) <= 2f)
            return;

        Vector3 endPoint = enemy.targetKnewPos + ((enemy.targetKnewPos - enemy.transform.position).normalized * 3);

        if(Vector3.Distance(targetAnchorPos,enemy.targetKnewPos) > 3)
        {
            curvePath.RegenaratePath(endPoint,enemy.transform.position);
            targetAnchorPos = enemy.targetKnewPos;
        }

        if (curvePath._curvePoint.Count > 0)
            if (enemyCommandAPI.MoveToPosition(curvePath._curvePoint.Peek(), 1, 1.5f))
            {
                enemyCommandAPI.FreezPosition();
                curvePath._curvePoint.Dequeue();
            }
    }
}
