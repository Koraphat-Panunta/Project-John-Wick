using System;
using UnityEngine;

public class PlayPoseAnimationNodeLeaf : PlayAnimationNodeLeaf
{
    protected AnimationPoseTimeNormalized animationPoseTimeNormalized;
    public bool isLoop;
    public float duration;

    public float timer { get; protected set; }
    public float startNormalized { get; protected set; }

    protected override bool isComplete 
    { 
        get 
        {
            if(this.isLoop)
                return false;

            if(this.timer >= this.duration)
                return true;

            return false;
        } 
    }
    
    public PlayPoseAnimationNodeLeaf
        (
        Func<bool> preCondition
        , Animator animator
        , string stateName
        , int layer
        , float transitionDurationNormalized
        , AnimationPoseTimeNormalized animationPoseTimeNormalized
        , float duration
        , bool isLoop
        ) 
        : base(preCondition, animator, stateName, layer, transitionDurationNormalized, 0)
    {
        this.animationPoseTimeNormalized = animationPoseTimeNormalized;
        this.duration = duration;
        this.isLoop = isLoop;

        this.startNormalized = 0;
    }

    public override void Enter()
    {
        this.timer = 0;
        base.Enter();
    }

    public override void UpdateNode()
    {
        if (this.timer < this.duration)
        {
            this.timer += Time.deltaTime;
        }
        else
        {
            if (this.isLoop)
                this.timer = this.startNormalized * this.duration;
        }

        this.animationPoseTimeNormalized.timeNormal = (this.timer/this.duration);
        base.UpdateNode();
    }

    public void SetStartNormalized(float setNormalized) => this.startNormalized = setNormalized;
    public void SetDuration(float duration) => this.duration = duration;
}
public class AnimationPoseTimeNormalized 
{
    public float timeNormal;
}
