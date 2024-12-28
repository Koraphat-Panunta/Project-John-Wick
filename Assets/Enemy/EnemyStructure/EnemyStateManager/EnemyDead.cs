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
        MotionControlManager motionControlManager = enemyState.enemy.motionControlManager;

        motionControlManager.ChangeMotionState(motionControlManager.ragdollMotionState);
        enemyState.enemy.isDead = true;
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
        enemyState.enemy.agent.speed = 0;
        enemyState.enemy.agent.acceleration = 0;
        base.StateUpdate(enemyState);
    }
}
