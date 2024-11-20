using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingTactic : IEnemyTactic
{
    private Enemy enemy;
    private bool isSeeTargetPos;
    private IEnemyFiringPattern enemyFiringPattern;
    private EnemyFindingCover findingCover;
    private float costRate;
    private float exitStateCost = 70;
    private float findCoverFrequency = 2;
    public HoldingTactic(Enemy enemy)
    {
        this.enemy = enemy;
        enemyFiringPattern = new NormalFiringPattern(enemy);
        findingCover = new EnemyFindingCover();
        costRate = Random.Range(8, 15f);
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.Holding);
        Debug.Log(enemy + " EnterHolding");
        enemy.isIncombat = true;
    }
    public void Manufacturing()
    {
        enemy.enemyLookForPlayer.Recived();
        if (enemy.enemyLookForPlayer.IsSeeingPlayer)
        {
            enemy.enemyComunicate.SendNotify(EnemyComunicate.NotifyType.SendTargetLocation, 18f);
            isSeeTargetPos = true;
        }
        else
        {
            Ray ray = new Ray(enemy.rayCastPos.position, (enemy.Target.transform.position - enemy.rayCastPos.position).normalized);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Vector3.Distance(enemy.rayCastPos.position, enemy.Target.transform.position), LayerMask.GetMask("Default") + enemy.targetMask))
            {
                if (hitInfo.collider.gameObject.layer == enemy.targetMask)
                {
                    isSeeTargetPos = true;
                }
                else
                {
                    isSeeTargetPos = false;
                }
            }
            else
            {
                isSeeTargetPos = true;
            }
        }
        if(isSeeTargetPos == true)
        {
            enemy.weaponCommand.AimDownSight();
            enemyFiringPattern.Performing();
            enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._idle);
            Vector3 targetDir = enemy.Target.transform.position.normalized - enemy.transform.position.normalized;
            new RotateObjectToward().RotateTowards(enemy.Target, enemy.gameObject, 6);
        }
        else if(isSeeTargetPos == false)
        {
            enemy.weaponCommand.LowReady();
            enemy.agent.destination = Vector3.Cross(enemy.Target.transform.position - enemy.transform.position, Vector3.up);
            enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._move);
            new RotateObjectToward().RotateTowards(enemy.Target, enemy.gameObject, 6);
        }
        if(findCoverFrequency <=0)
        {
            //if (findingCover.FindingCover(enemy))
            //{
            //    enemy.currentTactic = new TakeCoverTactic(enemy);
            //}
            findCoverFrequency = 2;
        }
        if (findingCover.FindingCover(enemy))
        {
            enemy.currentTactic = new TakeCoverTactic(enemy);
        }
        findCoverFrequency -= Time.deltaTime;
        if(enemy.cost >= exitStateCost)
        {
            //exit tactic
            enemy.currentTactic = new FlankingTactic(enemy);
        }
        enemy.cost += costRate * Time.deltaTime;
    }
}
