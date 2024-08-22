using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : EnemyState
{
    public EnemyDead()
    {

    }
    public override void StateEnter(EnemyStateManager enemyState)
    {
        enemyState.enemy.GetComponent<RagdollAnimation>().EnableRagdoll();
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
        base.StateUpdate(enemyState);
    }
}
