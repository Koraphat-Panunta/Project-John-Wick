using System;
using UnityEngine;

public class TakeCoverEnemyActionNodeLeaf : EnemyActionNodeLeaf
{
    private CoverPoint coverPoint => enemy.coverPoint;

    private float elapseCoverTime;
    private float coverTimePerRound = 6;
    private float peekTime =2.5f;
    public TakeCoverEnemyActionNodeLeaf(Enemy enemy, EnemyCommandAPI enemyCommandAPI, Func<bool> preCondition, EnemyActionNodeManager enemyActionNodeManager) : base(enemy, enemyCommandAPI, preCondition, enemyActionNodeManager)
    {
    }
    public override void Exit()
    {
        enemyCommandAPI.GetOffCover();
        base.Exit();
    }
    public override void Enter()
    {            
        base.Enter();
    }
    public override void UpdateNode()
    {
        Vector3 enemyToCover = coverPoint.coverPos.position - enemy.transform.position;
        Vector3 enemyToTarget = enemy.targetKnewPos - enemy.transform.position;


        if (enemy.isInCover == false)
        {
            //if (Vector3.Dot(enemyToTarget, enemyToCover) > 6.5f)
            //{
            //    enemyCommandAPI.SprintToCover(enemy.coverPoint);
            //    enemyCommandAPI.LowReady();
            //}
            //else
            //{
                enemyCommandAPI.MoveToTakeCover(this.coverPoint, 1);
                enemyCommandAPI.AimDownSight(enemy.targetKnewPos, enemy.aimingRotateSpeed);
                enemyCommandAPI.NormalFiringPattern.Performing();
            //}
        }
        else
        {
            elapseCoverTime += Time.deltaTime;
            if(elapseCoverTime >= coverTimePerRound)
                elapseCoverTime = 0;

            if (elapseCoverTime < peekTime)
                Cover();
            else //elapseCoverTime >= peekTime
                Peek();
        }

        base.UpdateNode();
    }

    private void Cover()
    {
        enemyCommandAPI.LowReady();
    }
    private void Peek()
    {
        switch (enemyActionNodeManager.curCombatPhase)
        {
            case IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert:
                {
                    enemyCommandAPI.AimDownSight(enemy.targetKnewPos, enemy.aimingRotateSpeed);
                    enemyCommandAPI.NormalFiringPattern.Performing();
                }
                break;
            case IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware:
                {
                    enemyCommandAPI.AimDownSight(enemy.targetKnewPos, enemy.aimingRotateSpeed);
                }
                break;
            case IEnemyActionNodeManagerImplementDecision.CombatPhase.Chill: enemyCommandAPI.LowReady(); break;
        }
    }
  
}
