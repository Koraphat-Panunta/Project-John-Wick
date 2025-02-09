using System;
using System.Collections.Generic;
using UnityEngine;

public class Sprint_Enemy_AnimationNodeLeaf : EnemyStateLeafNode
{
    private Animator animator;
    private string stateName = "Sprint";
    private int stateLayer = 0;
    public Sprint_Enemy_AnimationNodeLeaf(Enemy enemy,Animator animator, Func<bool> preCondition) : base(enemy, preCondition)
    {
        this.animator = animator;
    }

    public override void Enter()
    {
        animator.CrossFade(stateName, 0.5f, stateLayer,0);
        base.Enter();
    }
    public override void UpdateNode()
    {
        animator.SetLayerWeight(1,0);
        base.UpdateNode();
    }

}
