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
        Debug.Log("Idle Enter");
    }

    public override void StateExit(EnemyStateManager enemyState)
    {
       
    }

    public override void StateFixedUpdate(EnemyStateManager enemyState)
    {
       
    }

    public override void StateUpdate(EnemyStateManager enemyState)
    {
        Animator animator = enemyState.EnemyAction._enemy.animator;
        NavMeshAgent agent = enemyState.EnemyAction._enemy.agent;
        GameObject MyEnemy = enemyState.EnemyAction._enemy.gameObject;
        animator.SetFloat("Vertical", Mathf.Lerp(animator.GetFloat("Vertical"), 0, 2 * Time.deltaTime));
        animator.SetFloat("Horizontal", Mathf.Lerp(animator.GetFloat("Horizontal"), 0, 2 * Time.deltaTime));
        Debug.Log("Idle");
    }

   
}
