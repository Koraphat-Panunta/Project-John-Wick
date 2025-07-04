using System;
using UnityEngine;

public class TakeCoverEnemyActionNodeLeaf : EnemyActionNodeLeaf
{
    private CoverPoint coverPoint => enemy.coverPoint;

    private float elapseCoverTime;
    private float coverTimePerRound = 6;
    private float peekTime =2.5f;

    private float coverPointThreatDistance = 1.25f;
    public TakeCoverEnemyActionNodeLeaf(Enemy enemy, EnemyCommandAPI enemyCommandAPI, Func<bool> preCondition, EnemyActionNodeManager enemyActionNodeManager) : base(enemy, enemyCommandAPI, preCondition, enemyActionNodeManager)
    {
    }
    public override bool IsReset()
    {
        Vector3 dirTotargetKnow = enemy.targetKnewPos - coverPoint.coverPos.position;

        float coverDirDotTargetDir = Vector3.Dot(coverPoint.coverDir * this.coverPointThreatDistance, dirTotargetKnow);
        float coverDirDotCoverDir = Vector3.Dot(coverPoint.coverDir * this.coverPointThreatDistance, coverPoint.coverDir * this.coverPointThreatDistance);

        Debug.Log("coverDirDotTargetDir = " + coverDirDotTargetDir);
        Debug.Log("coverDirDotCoverDir = " + coverDirDotCoverDir);

        if(coverDirDotTargetDir < coverDirDotCoverDir)
            return true;

        return base.IsReset();
    }
    public override void Exit()
    {
        enemyCommandAPI.CheckOutCover();
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
           
                enemyCommandAPI.MoveToTakeCover(this.coverPoint, 1);
                enemyCommandAPI.AimDownSight(enemy.targetKnewPos, enemy.aimingRotateSpeed);
                enemyCommandAPI.NormalFiringPattern.Performing();
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
