using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAgent : EnemyMove
{
    public override void StateEnter(EnemyStateManager enemyState)
    {
        enemyState.enemy.agent.speed = 5;
        enemyState.enemy.agent.acceleration = 5;
        base.StateEnter(enemyState);
    }

    public override void StateExit(EnemyStateManager enemyState)
    {
        enemyState.enemy.agent.speed = 0;
        enemyState.enemy.agent.acceleration = 0;
        base.StateExit(enemyState);
    }

    public override void StateFixedUpdate(EnemyStateManager enemyState)
    {
        base.StateFixedUpdate(enemyState);
    }

    public override void StateUpdate(EnemyStateManager enemyState)
    {
        Debug.Log("MoveWithAgent"+ enemyState.enemy.agent.speed+"||"+enemyState.enemy.agent.acceleration);
        base.StateUpdate(enemyState);
    }
}
