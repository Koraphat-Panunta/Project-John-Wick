using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flanking : IEnemyTactic
{
    private Enemy enemy;
    private EnemyStateManager enemyStateManager;
    public Flanking(Enemy enemy)
    {
        this.enemy = enemy;
        if (enemy.enemyPath._markPoint.Count <= 0)
        {
            enemy.enemyPath.GenaratePath(enemy.Target.transform.position, enemy.gameObject.transform.position);
        }
        enemyStateManager = enemy.enemyStateManager;
        enemyStateManager.ChangeState(enemyStateManager._move);
    }
    public void Manufacturing()
    {
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
