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
        this.isTriggerReset = false;
        //Debug.Log("Enter Animation State " + stateName);
        animator.CrossFadeInFixedTime(stateName,transitionDurationNormalized,layer,transitionOffsetNormalized);
        base.Enter();
    }
    public override void Exit()
    {
        this.isTriggerReset = false;
        base.Exit();
    }

    public bool isTriggerReset;

    public void TriggerReset() => this.isTriggerReset = true;

    public override bool IsReset()
    {
        if(this.isTriggerReset)
            return true;

        return base.IsReset();
    }
}
