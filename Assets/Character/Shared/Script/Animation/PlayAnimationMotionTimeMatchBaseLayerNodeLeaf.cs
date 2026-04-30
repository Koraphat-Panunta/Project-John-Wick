using System;
using UnityEngine;

public class PlayAnimationMotionTimeMatchBaseLayerNodeLeaf : AnimationNodeLeaf
{
    protected Animator animator;
    public string stateName;
    protected int layer;
    protected int baseLayer;
    protected AnimationPoseTimeNormalized motionTimeParameter;
    protected float transitionDurationNormalized;

    public PlayAnimationMotionTimeMatchBaseLayerNodeLeaf(
        Func<bool> preCondition,
        Animator animator,
        string stateName,
        int layer,
        int baseLayer,
        AnimationPoseTimeNormalized motionTimeParameter,
        float transitionDurationNormalized) : base(preCondition)
    {
        this.animator = animator;
        this.stateName = stateName;
        this.layer = layer;
        this.baseLayer = baseLayer;
        this.motionTimeParameter = motionTimeParameter;
        this.transitionDurationNormalized = transitionDurationNormalized;
    }

    public override void Enter()
    {
        animator.CrossFadeInFixedTime(stateName, transitionDurationNormalized, layer, 0);
        base.Enter();
    }

    public override void UpdateNode()
    {

        float baseNormalizedTime = animator.GetCurrentAnimatorStateInfo(baseLayer).normalizedTime;

        this.motionTimeParameter.timeNormal = Mathf.Repeat(baseNormalizedTime, 1f);
        base.UpdateNode();
    }
}
