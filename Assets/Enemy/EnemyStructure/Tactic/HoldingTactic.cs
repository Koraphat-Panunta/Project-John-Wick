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
        //enemy.isInCombat = true;
    }
    public void Manufacturing()
    {

        if (enemy.findingTargetComponent.FindTarget(out GameObject target))
        {
            enemy.enemyComunicate.SendNotify(EnemyComunicate.NotifyType.SendTargetLocation, 18f);
            isSeeTargetPos = true;
        }
        else
        {
            Ray ray = new Ray(enemy.rayCastPos.position, (enemy.targetKnewPos - enemy.rayCastPos.position).normalized);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Vector3.Distance(enemy.rayCastPos.position, enemy.targetKnewPos), LayerMask.GetMask("Default") + enemy.targetMask))
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
            enemy.enemyController.Freez();
            Vector3 targetDir = enemy.targetKnewPos.normalized - enemy.transform.position.normalized;
            enemy.enemyController.Rotate(targetDir,6);

        }
        else if(isSeeTargetPos == false)
        {
            enemy.weaponCommand.LowReady();
            enemy.agent.destination = Vector3.Cross(enemy.targetKnewPos - enemy.transform.position, Vector3.up);

            Vector3 dir = enemy.agent.steeringTarget - enemy.transform.position;
            enemy.enemyController.Move(dir, 1);
            Vector3 targetDir = enemy.targetKnewPos.normalized - enemy.transform.position.normalized;
            enemy.enemyController.Rotate(targetDir, 6);

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
