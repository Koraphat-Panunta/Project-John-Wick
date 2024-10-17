using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlankingTactic : IEnemyTactic
{
    private Enemy enemy;
    private EnemyStateManager enemyStateManager;
    private RotateObjectToward enemyRot;
    private EnemyWeaponCommand enemyWeaponCommand;
    private IEnemyFiringPattern enemyFiringPattern;
    private float backToSerchTiming = 2;
    private float cost_DrainRate = 4;
    public FlankingTactic(Enemy enemy)
    {
        this.enemy = enemy;
        enemy.enemyPath.GenaratePath(enemy.Target.transform.position, enemy.gameObject.transform.position);
        enemyStateManager = enemy.enemyStateManager;
        enemyStateManager.ChangeState(enemyStateManager._move);
        enemyRot = new RotateObjectToward();
        enemyWeaponCommand = enemy.enemyWeaponCommand;
        this.enemyFiringPattern = new NormalFiringPattern(enemy);
    }
    public void Manufacturing()
    {
        EnemyFindingCover enemyFindingCover = new EnemyFindingCover();
        if (enemyFindingCover.FindingCover(enemy))
        {
            Debug.Log("FoundCover");
        }
        else
        {
            Debug.Log("NOT FoundCover");
        }
        enemy.enemyLookForPlayer.Recived();
        if (enemy.cost < 34&&enemy.cost > Vector3.Distance(enemy.transform.position,enemy.Target.transform.position)*10)
        {
            //Change state to TakeCover
        }
        if (enemy.enemyLookForPlayer.IsSeeingPlayer == true)
        {
            enemyWeaponCommand.Aiming();
            enemyFiringPattern.Performing();
            enemy.enemyComunicate.SendNotify(EnemyComunicate.NotifyType.SendTargetLocation, 18f);
            enemy.cost -= cost_DrainRate*Time.deltaTime;
        }
        else
        {
            if (enemy.enemyPath._markPoint.Count<=0)
            {
                enemyWeaponCommand.LowReady();
                backToSerchTiming -= Time.deltaTime;
                if (backToSerchTiming <= 0)
                {
                    backToSerchTiming = 2;
                    enemy.currentTactic = new SerchingTactic(enemy);
                }
            }
            else
            {
                if (enemy.enemyLookForPlayer.lostSightTiming < 4f)
                {
                    enemyFiringPattern.Performing();
                    enemy.cost -= cost_DrainRate * Time.deltaTime;
                }
            }
        }
        enemyRot.RotateTowards(enemy.Target, enemy.gameObject, 6);
        if (Vector3.Distance(enemy.Target.transform.position,enemy.gameObject.transform.position) < 2.5f)
        {
            enemyStateManager.ChangeState(enemyStateManager._idle);
        }
        else
        {
            enemyStateManager.ChangeState(enemyStateManager._move);
        }
        enemy.enemyPath.UpdateTargetPos(enemy.Target.transform.position,enemy.gameObject.transform.position);
    }
}
