using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunFuNodeLeaf : PlayerActionNodeLeaf
{
    public abstract bool isTransitionAble { get; set; }
    public abstract float exitTime { get; }
    public abstract float transitionTime { get;}
    public float timer { get; private set; }
    protected AnimationClip gunFuAnimationClip;
    public GunFuNodeLeaf(Player player,AnimationClip animationClip) : base(player)
    {
        gunFuAnimationClip = animationClip;
    }

    public override List<PlayerNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        timer = 0;
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
        timer += Time.deltaTime;
    }
}
