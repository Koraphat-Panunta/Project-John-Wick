using System;
using UnityEngine;

public class PlayAnimationNodeLeaf : AnimationNodeLeaf
{
    protected Animator animator;
    public string stateName;
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
    public PlayAnimationNodeLeaf(Func<bool> preCondition, Animator animator, string stateName, int layer, float transitionDurationNormalized) : this(preCondition,animator,stateName,layer,transitionDurationNormalized,0)
    {

    }

    public override void Enter()
    {
        //Debug.Log("Enter Animation State " + stateName);
        animator.CrossFade(stateName,transitionDurationNormalized,layer,transitionOffsetNormalized);
        base.Enter();
    }
}
