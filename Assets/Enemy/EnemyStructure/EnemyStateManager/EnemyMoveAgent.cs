
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveAgent : EnemyMove
{
    float agentMoveSpeed = 2;
    public override void StateEnter(EnemyStateManager enemyState)
    {
       
        base.StateEnter(enemyState);
    }

    public override void StateExit(EnemyStateManager enemyState)
    {
        
        base.StateExit(enemyState);
    }

    public override void StateFixedUpdate(EnemyStateManager enemyState)
    {
        base.StateFixedUpdate(enemyState);
    }

    public override void StateUpdate(EnemyStateManager enemyState)
    {
        Enemy enemy = enemyState.enemy;
        NavMeshAgent agent = enemy.agent;

        Vector3 moveDir = (agent.steeringTarget - enemy.transform.position).normalized * Time.deltaTime* agentMoveSpeed;
        agent.Move(moveDir);
        base.StateUpdate(enemyState);
    }
}
