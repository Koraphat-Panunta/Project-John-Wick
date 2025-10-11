using System;
using UnityEngine;

public class PlayAnimationBaseStateOffsetNodeLeaf : AnimationNodeLeaf
{
    protected Animator animator;
    public string stateName;
    protected int layer;
    protected int baseLayer;
    protected float transitionDurationNormalized;
    protected float transitionOffsetNormalized => animator.GetCurrentAnimatorStateInfo(baseLayer).normalizedTime;
    public PlayAnimationBaseStateOffsetNodeLeaf(Func<bool> preCondition,Animator animator, string stateName, int layer,int baseLayer, float transitionDurationNormalized) : base(preCondition)
    {
        this.animator = animator;
        this.stateName = stateName;
        this.layer = layer;
        this.baseLayer = baseLayer;
        this.transitionDurationNormalized = transitionDurationNormalized;

    }
    public override void Enter()
    {
        //Debug.Log("Enter Animation State " + stateName);
        animator.CrossFade(stateName, transitionDurationNormalized, layer, transitionOffsetNormalized);
        base.Enter();
    }
}
