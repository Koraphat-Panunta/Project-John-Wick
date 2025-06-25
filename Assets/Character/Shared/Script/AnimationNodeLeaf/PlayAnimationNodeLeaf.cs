using System;
using UnityEngine;

public class PlayAnimationNodeLeaf : AnimationNodeLeaf
{
    protected Animator animator;
    protected string stateName;
    protected int layer;
    protected float transitionDurationNormalized;
    protected float transitionOffsetNormalized;
    public PlayAnimationNodeLeaf(Func<bool> preCondition, Animator animator,string stateName,int layer,float transitionDurationNormalized,float transitionOffsetNormalized) : base(preCondition)
    {
        this.animator = animator;
        this.stateName = stateName;
        this.layer = layer; 
        this.transitionDurationNormalized = transitionDurationNormalized;
        this.transitionOffsetNormalized = transitionOffsetNormalized;
    }

    public override void Enter()
    {
        animator.CrossFade(stateName,transitionDurationNormalized,layer,transitionOffsetNormalized);
        base.Enter();
    }
}
