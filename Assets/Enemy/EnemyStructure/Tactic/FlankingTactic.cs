using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlankingTactic : IEnemyTactic
{
    private Enemy enemy;
    private EnemyStateManager enemyStateManager;
    private RotateObjectToward enemyRot;
    //private IEnemyFiringPattern enemyFiringPattern;
    private float backToSerchTiming = 2;
    private float cost_DrainRate;
    public FlankingTactic(Enemy enemy)
    {
        this.enemy = enemy;
        enemy.enemyPath.GenaratePath(enemy.targetKnewPos, enemy.gameObject.transform.position);
        enemyStateManager = enemy.enemyStateManager;
        enemyStateManager.ChangeState(enemyStateManager._move);
        enemyRot = new RotateObjectToward();
        //this.enemyFiringPattern = new NormalFiringPattern(enemy);
        cost_DrainRate = Random.Range(9,15);
        Debug.Log(enemy+" EnterFlanking");
    }
    public void Manufacturing()
    {
        if (enemy.cost < 34/*&&enemy.cost > Vector3.Distance(enemy.transform.position,enemy.Target.transform.position)*2*/)
        {
            enemy.currentTactic = new TakeCoverTactic(enemy);
        }
        if (enemy.findingTargetComponent.FindTarget(out GameObject target) == true)
        {
            //Shoot
            enemy.weaponCommand.AimDownSight();
            //enemyFiringPattern.Performing();
            enemy.enemyComunicate.SendNotify(EnemyComunicate.NotifyType.SendTargetLocation, 18f);
            enemy.cost -= cost_DrainRate * Time.deltaTime;

        }
        else
        {
            if (enemy.enemyPath._markPoint.Count <= 0)
            {
                enemy.weaponCommand.LowReady();
                backToSerchTiming -= Time.deltaTime;
                if (backToSerchTiming <= 0)
                {
                    backToSerchTiming = 2;
                    enemy.currentTactic = new SerchingTactic(enemy);
                }
            }
            else
            {
                //if (enemy.enemyLookForPlayer.lostSightTiming < 4f)
                //{
                //    enemyFiringPattern.Performing();
                //    enemy.cost -= cost_DrainRate * Time.deltaTime;
                //}
            }
        }
        enemyRot.RotateTowardsObjectPos(enemy.targetKnewPos, enemy.gameObject, 6);
        if (Vector3.Distance(enemy.targetKnewPos,enemy.gameObject.transform.position) < 2.5f)
        {
            enemyStateManager.ChangeState(enemyStateManager._idle);
        }
        else
        {
            enemyStateManager.ChangeState(enemyStateManager._move);
        }
        enemy.enemyPath.UpdateTargetPos(enemy.targetKnewPos,enemy.gameObject.transform.position);
    }
}
