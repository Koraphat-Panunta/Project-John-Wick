using System;
using System.Collections.Generic;
using UnityEngine;


public class IdleMove_Enemy_AnimationNodeLeaf : EnemyStateLeafNode
{
    private Animator animator;
    private string stateName = "Move/Idle";
    private int stateLayer = 0;
    public IdleMove_Enemy_AnimationNodeLeaf(Enemy enemy,Animator animator, Func<bool> preCondition, Func<bool> isReset) : base(enemy, preCondition, isReset)
    {
        this.animator = animator;
    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        Debug.Log("Move/Idle CrossFafe");
        animator.CrossFade(stateName, 0.5f, stateLayer);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override bool PreCondition()
    {
        return base.PreCondition();
    }

    public override void Update()
    {
        animator.SetLayerWeight(1, animator.GetLayerWeight(1) + Time.deltaTime * 3);
        base.Update();
    }
}
