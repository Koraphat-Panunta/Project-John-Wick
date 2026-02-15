using System;
using UnityEngine;

public class PlayPoseAnimationNodeLeaf : PlayAnimationNodeLeaf
{
    public PlayPoseAnimationScriptableObject playPoseAnimationScriptableObject;

    protected AnimationPoseTimeNormalized animationPoseTimeNormalized;

    public bool _isLoop 
    { 
        get => this.playPoseAnimationScriptableObject != null ? this.playPoseAnimationScriptableObject.isLoop : this.isLoopValue; 
        set => this.isLoopValue = value;
    }
    private bool isLoopValue;

    public float _isLoopAtNormalized 
    {
        get => this.playPoseAnimationScriptableObject != null ? this.playPoseAnimationScriptableObject.isLoopAtNormalized : this.isLoopAtNormalizedValue;
        set => this.isLoopAtNormalizedValue = value;
    }
    private float isLoopAtNormalizedValue;

    public float _duration 
    {
        get => this.playPoseAnimationScriptableObject != null ? this.playPoseAnimationScriptableObject.duration : this.durationValue;
        set => this.durationValue = value;
    }
    private float durationValue;

    public float _startNormalized 
    {
        get => this.playPoseAnimationScriptableObject != null ? this.playPoseAnimationScriptableObject.startNormalized : this.startNormalizedValue; 
        set => this.startNormalizedValue = value;
    }
    private float startNormalizedValue;

    public float timer { get; protected set; }

    public AnimationCurve _animationCurve 
    {
        get => this.playPoseAnimationScriptableObject != null ? this.playPoseAnimationScriptableObject.animationCurve : this.animationCurveValue;
        set => this.animationCurveValue = value;
    }
    private AnimationCurve animationCurveValue;

    protected override bool isComplete 
    { 
        get 
        {
            if(this._isLoop)
                return false;

            if(this.timer >= this._duration)
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
        this.durationValue = duration;
        this.isLoopValue = isLoop;
        this.isLoopAtNormalizedValue = 0;

        this.startNormalizedValue = 0;
    }
    public PlayPoseAnimationNodeLeaf
        (
        Func<bool> preCondition
        , Animator animator
        , string stateName
        , int layer
        , AnimationPoseTimeNormalized animationPoseTimeNormalized        
        , float transitionDurationNormalized
        , PlayPoseAnimationScriptableObject playPoseAnimationScriptableObject
        )
        : base(preCondition, animator, stateName, layer, transitionDurationNormalized, 0)
    {
        this.animationPoseTimeNormalized = animationPoseTimeNormalized;
        this.playPoseAnimationScriptableObject = playPoseAnimationScriptableObject;


    }

    public override void Enter()
    {
        this.timer = this._startNormalized * this._duration;
        this.animator.CrossFadeInFixedTime(stateName, transitionDurationNormalized, layer, transitionOffsetNormalized);

    }

    public override void UpdateNode()
    {
        if (this.timer < this._duration)
        {
            this.timer += Time.deltaTime;
        }
        else
        {
            if (this._isLoop)
                this.timer = this._isLoopAtNormalized * this._duration;
        }

        float t = (this.timer / this._duration);
        this.animationPoseTimeNormalized.timeNormal = (this._animationCurve != null?this._animationCurve.Evaluate(t):t);
        base.UpdateNode();
    }

    public void SetStartNormalized(float setNormalized) => this._startNormalized = setNormalized;
    public void SetDuration(float duration) => this._duration = duration;
}
public class AnimationPoseTimeNormalized 
{
    public float timeNormal;
}
