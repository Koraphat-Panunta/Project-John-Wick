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
        enemy.enemyLookForPlayer.Recived();
        if (enemy.enemyLookForPlayer.IsSeeingPlayer == true)
        {
            enemyWeaponCommand.Aiming();
            enemyFiringPattern.Performing();
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
