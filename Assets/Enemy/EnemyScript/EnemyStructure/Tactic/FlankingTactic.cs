using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlankingTactic : IEnemyTactic
{
    private Enemy enemy;
    //private EnemyStateManager enemyStateManager;
    //private RotateObjectToward enemyRot;
    private NormalFiringPattern enemyFiringPattern;
    private float backToSerchTiming = 2;
    private float cost_DrainRate;
    private EnemyCommandAPI enemyController;
    public FlankingTactic(Enemy enemy)
    {
        this.enemy = enemy;
        //enemy.enemyPath.GenaratePath(enemy.targetKnewPos, enemy.gameObject.transform.position);
        //enemyStateManager = _enemy.enemyStateManager;
        //enemyStateManager.ChangeState(enemyStateManager._move);
        //enemyRot = new RotateObjectToward();
        this.enemyFiringPattern = new NormalFiringPattern(enemy);
        cost_DrainRate = Random.Range(9,15);
        this.enemyController = enemy.enemyController;
        Debug.Log(enemy + " EnterFlanking");
    }
    public void Manufacturing()
    {
        if (enemy.cost < 34/*&&_enemy.cost > Vector3.Distance(_enemy.transform.position,_enemy.Target.transform.position)*2*/)
        {
            enemy.currentTactic = new TakeCoverTactic(enemy);
        }
        if (enemy.findingTargetComponent.FindTarget(out GameObject target) == true)
        {
            //Shoot

            Vector3 lookdir = (enemy.targetKnewPos - enemy.transform.position).normalized;

            enemy.enemyController.Rotate(lookdir, 6);
            enemy.weaponCommand.AimDownSight();
            enemyFiringPattern.Performing();
            enemy.enemyComunicate.SendNotify(EnemyComunicate.NotifyType.SendTargetLocation, 18f);
            enemy.cost -= cost_DrainRate * Time.deltaTime;

        }
        else
        {
            //if (enemy.enemyPath._markPoint.Count <= 0)
            //{
            //    enemy.weaponCommand.LowReady();
            //    backToSerchTiming -= Time.deltaTime;
            //    if (backToSerchTiming <= 0)
            //    {
            //        backToSerchTiming = 2;
            //        enemy.currentTactic = new SerchingTactic(enemy);
            //    }
            //}
            //else
            //{
            //    //if (_enemy.enemyLookForPlayer.lostSightTiming < 4f)
            //    //{
            //    //    enemyFiringPattern.Performing();
            //    //    _enemy.cost -= cost_DrainRate * Time.deltaTime;
            //    //}
            //    if(enemy.combatOffensiveInstinct.myCombatPhase == CombatOffensiveInstinct.CombatPhase.Alert)
            //    {
            //        enemyFiringPattern.Performing();
            //        enemy.cost -= cost_DrainRate * Time.deltaTime;
            //    }
            //}
            Vector3 lookdir = (enemy.targetKnewPos - enemy.transform.position).normalized;
            enemy.enemyController.Rotate(lookdir, 6);
        }
        //enemyRot.RotateTowardsObjectPos(_enemy.targetKnewPos, _enemy.gameObject, 6);
        if (Vector3.Distance(enemy.targetKnewPos,enemy.gameObject.transform.position) < 2.5f)
        {
            enemyController.Freez();
        }
        else
        {
            Vector3 dir = enemy.agent.steeringTarget - enemy.transform.position;
            enemyController.Move(dir,1);
        }
        //enemy.enemyPath.UpdateTargetPos(enemy.targetKnewPos,enemy.gameObject.transform.position);
    }
}
