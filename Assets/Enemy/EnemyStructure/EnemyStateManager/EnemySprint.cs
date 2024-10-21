using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySprint : EnemyState
{
    Enemy enemy;
    RotateObjectToward objectToward;
    NavMeshAgent agent;
    Animator animator;
    public EnemySprint(Enemy enemy)
    {
        this.enemy = enemy;
        objectToward = new RotateObjectToward();
        this.agent = enemy.agent;
        animator = enemy.animator;
    }
    public override void StateEnter(EnemyStateManager enemyState)
    {
        animator.SetBool("IsSprinting",true);
        animator.speed = 1f;
        base.StateEnter(enemyState);
    }

    public override void StateExit(EnemyStateManager enemyState)
    {
        animator.SetBool("IsSprinting", false);
        animator.speed = 1f;
        base.StateExit(enemyState);
    }

    public override void StateFixedUpdate(EnemyStateManager enemyState)
    {
       
        base.StateFixedUpdate(enemyState);
    }

    public override void StateUpdate(EnemyStateManager enemyState)
    {
        float rotSpeed = 8;
        Vector3 dir = (agent.steeringTarget - enemy.transform.position).normalized;
        objectToward.RotateTowards(dir, enemy.gameObject, rotSpeed);
        enemy.currentTactic.Manufacturing();
        base.StateUpdate(enemyState);
    }
}
