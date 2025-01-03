using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : EnemyState
{
    EnemyPath enemyPath;
    float moveSpeed = 1.78f; //By defualt

    public EnemyMove()
    {

    }
    public override void StateEnter(EnemyStateManager enemyState)
    {
        if (enemyPath == null)
        {
            enemyPath = enemyState.enemy.enemyPath;
        }
        if (enemyPath._markPoint.Count <= 0)
        {
            enemyPath.GenaratePath(enemyState.enemy.targetKnewPos, enemyState.enemy.gameObject.gameObject.transform.position);
        }
        enemyState.enemy.animator.speed = moveSpeed;
    }
    public override void StateExit(EnemyStateManager enemyState)
    {
        enemyState.enemy.animator.speed = 1; // Set back to defualt
    }
    public override void StateFixedUpdate(EnemyStateManager enemyState)
    {
       
    }
    public override void StateUpdate(EnemyStateManager enemyState)
    {
        Animator animator = enemyState.enemy.animator;
        NavMeshAgent agent = this.enemyPath.agent;
        GameObject MyEnemy = enemyState.enemy.gameObject;
        EnemyPath enemyPath = enemyState.enemy.enemyPath;
        enemyState.enemy.currentTactic.Manufacturing();
        if (enemyPath._markPoint.Count > 0)
        {
            if (agent.hasPath)//Move Enemy
            {
                Vector3 dir = agent.steeringTarget - MyEnemy.transform.position;
                Vector3 animDir = MyEnemy.transform.InverseTransformDirection(dir);
                animator.SetFloat("Vertical", animDir.z, 0.5f, Time.deltaTime);
                animator.SetFloat("Horizontal", animDir.x, 0.1f, Time.deltaTime);
                //MyEnemy.transform.rotation = Quaternion.RotateTowards(MyEnemy.transform.rotation, Quaternion.LookRotation(dir), 180 * Time.deltaTime);
            }
        }
        EnemyDebuger.curPos = agent.destination;
        base.StateUpdate(enemyState);
    }
    
}
