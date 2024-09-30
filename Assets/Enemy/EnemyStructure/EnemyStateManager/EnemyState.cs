using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState 
{
    public EnemyState()
    {

    }

    public virtual void StateEnter(EnemyStateManager enemyState)
    {

    }
    public virtual void StateExit(EnemyStateManager enemyState)
    {

    }
    public virtual void StateUpdate(EnemyStateManager enemyState)
    {
        if (enemyState.enemy.GetHP() <= 0)
        {
            enemyState.ChangeState(enemyState.enemyDead);
        }
    }
    public virtual void StateFixedUpdate(EnemyStateManager enemyState)
    {

    }
}
