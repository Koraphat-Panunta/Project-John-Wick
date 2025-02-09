using System;
using System.Collections.Generic;
using UnityEngine;


public class IdleMove_Enemy_AnimationNodeLeaf : EnemyStateLeafNode
{
    private Animator animator;
    private string stateName = "Move/Idle";
    private int stateLayer = 0;
    public IdleMove_Enemy_AnimationNodeLeaf(Enemy enemy,Animator animator, Func<bool> preCondition) : base(enemy, preCondition)
    {
        this.animator = animator;
    }

    

    public override void Enter()
    {
        Debug.Log("Move/Idle CrossFafe");
        animator.CrossFade(stateName, 0.5f, stateLayer,0);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }


    public override void UpdateNode()
    {
        animator.SetLayerWeight(1, animator.GetLayerWeight(1) + Time.deltaTime * 3);
        base.UpdateNode();
    }
}
