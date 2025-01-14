using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationNodeLeaf : AnimationNode
{
    protected Animator animator;
    public AnimationNodeLeaf(EnemyAnimation enemyAnimation) : base(enemyAnimation)
    {
    }
    public AnimationNodeLeaf(EnemyAnimation enemyAnimation, Animator animator) : base(enemyAnimation)
    {
        this.animator = animator;
    }

    public override List<AnimationNode> childNode { get; set ; }
    protected override Func<bool> preCondidtion { get; set; }
    protected Func<bool> isReset { get; set; }

    public virtual void Enter()
    {

    }
    public virtual void Exit()
    {

    }
    public override void FixedUpdate()
    {
    }

    public override bool IsReset()
    {
        return isReset.Invoke();
    }

    public override bool PreCondition()
    {
        return preCondidtion.Invoke();
    }

    public override void Update()
    {
    }
}
