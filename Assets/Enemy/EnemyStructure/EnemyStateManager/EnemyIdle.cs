using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIdle : EnemyState
{
    public EnemyIdle()
    {
    }
    public override void StateEnter(EnemyStateManager enemyState)
    {
        //if(enemyState.enemy.enemyPath != null)
        //{
        //    enemyState.enemy.enemyPath.ResetPath();
        //}
    }

    public override void StateExit(EnemyStateManager enemyState)
    {
       
    }

    public override void StateFixedUpdate(EnemyStateManager enemyState)
    {
       
    }

    public override void StateUpdate(EnemyStateManager enemyState)
    {
        Animator animator = enemyState.enemy.animator;
        NavMeshAgent agent = enemyState.enemy.agent;
        GameObject MyEnemy = enemyState.enemy.gameObject;
        enemyState.enemy.currentTactic.Manufacturing();
        animator.SetFloat("Vertical", Mathf.Lerp(animator.GetFloat("Vertical"), 0, 2 * Time.deltaTime));
        animator.SetFloat("Horizontal", Mathf.Lerp(animator.GetFloat("Horizontal"), 0, 2 * Time.deltaTime));
    }

   
}
