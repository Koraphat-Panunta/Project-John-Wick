using System;
using UnityEngine;

public class SetLayerAnimationNodeLeaf : AnimationNodeLeaf
{

    protected Animator animator;
    protected int layer;
    protected float speed;
    protected float targetWeight;

    public SetLayerAnimationNodeLeaf(Func<bool> preCondition,Animator animator,int layer,float enableSpeed,float targetWeight) : base(preCondition)
    {
        this.animator = animator;
        this.layer = layer;
        this.speed = enableSpeed;
        this.targetWeight = targetWeight;
    }
    public override void UpdateNode()
    {

        float weight = Mathf.MoveTowards(animator.GetLayerWeight(layer), targetWeight, speed*Time.deltaTime);
        animator.SetLayerWeight(layer,weight);

        base.UpdateNode();
    }
}
