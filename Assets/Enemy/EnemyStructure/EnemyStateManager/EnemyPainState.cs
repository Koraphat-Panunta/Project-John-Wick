using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPainState : EnemyState
{
    public IEnemyHitReaction hitReaction;
    float Timeimg;
    public EnemyPainState()
    {
    }

    public override void StateEnter(EnemyStateManager enemyState)
    {
    }
    public void StateEnter(EnemyStateManager enemyState,IEnemyHitReaction hitReaction)
    {
        Timeimg = 0;
        enemyState.enemy.animator.SetLayerWeight(1, 0);
        enemyState.enemy.animator.SetLayerWeight(2, 0);
        enemyState.enemy.animator.SetLayerWeight(3, 0);
        this.hitReaction = hitReaction;
        this.hitReaction.Enter(enemyState);
        StateEnter(enemyState);
    }

    public override void StateExit(EnemyStateManager enemyState)
    {
    }

    public override void StateFixedUpdate(EnemyStateManager enemyState)
    {
       
    }

    public override void StateUpdate(EnemyStateManager enemyState)
    {
        Timeimg += Time.deltaTime;
        Debug.Log(Timeimg);
        this.hitReaction.Performed(enemyState);
    }
}
