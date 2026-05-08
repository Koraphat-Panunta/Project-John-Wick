using System;
using UnityEngine;

public class ApprouchingTargetEnemyActionNodeLeaf : EnemyActionNodeLeaf
{
    private EnemyDecisionContext enemyActionNodeManager;
    public EnemyMoveCurvePath curvePath;
    private const float MIN_CURVE_MOVE = 4;
    private const float MAX_CURVE_MOVE = 8;
    private const float MIN_APPROUCH_TIME = 6;
    private const float MAX_APPROUCH_TIME = 9;
    private float approuchingTime;
    private float approuchCooldOWN;
    public ApprouchingTargetEnemyActionNodeLeaf(
        Enemy enemy, EnemyCommandAPI enemyCommandAPI, Func<bool> preCondition, EnemyDecision enemyDecision, EnemyDecisionContext enemyDecisionContext) : base(enemy, enemyCommandAPI, preCondition, enemyDecision)
    {
        curvePath = new EnemyMoveCurvePath(MIN_CURVE_MOVE,MAX_CURVE_MOVE);
        this.enemyActionNodeManager = enemyDecisionContext;
    }

    public override void Enter()
    {
        Vector3 endPoint = this.enemy.targetKnowPos + ((this.enemy.targetKnowPos - enemy.transform.position).normalized * 3);
        curvePath.RegenaratePath(endPoint, enemy.transform.position);
        targetAnchorPos = this.enemy.targetKnowPos;
        if (enemyCommandAPI.enemyAutoDefendCommand.dodgeCoolDownTimer <= 0
            && curvePath.TryGetCurvePoint(out Vector3 _curvePoint))
            enemyCommandAPI.Dodge(_curvePoint);

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

        switch (enemyActionNodeManager.combatPhase)
        {
            case CombatPhase.Alert:
                {
                    enemyCommandAPI.AimDownSight(this.enemy.targetKnowPos);
                    enemyCommandAPI.NormalFiringPattern.Performing();

                }
                break;
            case CombatPhase.Aware:
                {
                    enemyCommandAPI.AimDownSight(this.enemy.targetKnowPos);
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
        if(Vector3.Distance(enemy.transform.position,this.enemy.targetKnowPos) <= 2f)
            return;

        Vector3 endPoint = this.enemy.targetKnowPos + ((this.enemy.targetKnowPos - enemy.transform.position).normalized * 3);

        if(Vector3.Distance(targetAnchorPos,this.enemy.targetKnowPos) > 3)
        {
            curvePath.RegenaratePath(endPoint,enemy.transform.position);
            targetAnchorPos = this.enemy.targetKnowPos;
        }

        if (curvePath.TryGetCurvePoint(out Vector3 _curvePoint))
            if (enemyCommandAPI.MoveToPosition(_curvePoint, 1, 1.5f))
            {
                enemyCommandAPI.FreezPosition();
                curvePath.DeQueueCurvePoint();
            }
    }
}
