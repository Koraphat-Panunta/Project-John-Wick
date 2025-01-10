using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class BodyHitNormalReaction : IEnemyHitReaction
{
    private Enemy enemy;
    private Animator animator;
    private bool animationIsPerformded = false;
    public BodyHitNormalReaction(Enemy enemy)
    {
        this.enemy = enemy;
        this.animator = enemy.animator;
    }


    public override void End(EnemyStateManager enemyState)
    {
        enemyState.ChangeState(enemyState._move);
    }
    public override void Enter(EnemyStateManager enemyState)
    {
        animator.SetTrigger(this.GetType().Name);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(this.GetType().Name) == false)
        {
            animationIsPerformded =false;
        }
    }
    public override void Performed(EnemyStateManager enemyState)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(this.GetType().Name))
        {
            animationIsPerformded = true;
        }
        if(animationIsPerformded == true) 
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(this.GetType().Name) == false && animator.GetAnimatorTransitionInfo(0).IsName("Enter->" + this.GetType().Name) == false)
            {
                End(enemyState);
            }
        }
       
    }
}
