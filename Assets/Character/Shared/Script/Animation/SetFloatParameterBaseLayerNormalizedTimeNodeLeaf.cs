using System;
using UnityEngine;

public class SetFloatParameterBaseLayerNormalizedTimeNodeLeaf : AnimationNodeLeaf
{
    protected Animator animator;
    protected int baseLayer;
    protected string parameterName;

    public SetFloatParameterBaseLayerNormalizedTimeNodeLeaf(
        Func<bool> preCondition,
        Animator animator,
        int baseLayer,
        string parameterName) : base(preCondition)
    {
        this.animator = animator;
        this.baseLayer = baseLayer;
        this.parameterName = parameterName;
    }

    public override void UpdateNode()
    {
        float baseNormalizedTime = animator.GetCurrentAnimatorStateInfo(baseLayer).normalizedTime;
        animator.SetFloat(parameterName, Mathf.Repeat(baseNormalizedTime, 1f));
        base.UpdateNode();
    }
}
