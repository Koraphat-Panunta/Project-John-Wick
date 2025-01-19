using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunFuHitNodeLeaf : PlayerActionNodeLeaf ,IGunFuNode
{
    public float _transitionAbleTime_Nornalized { get; set; }
    public float _exitTime_Normalized { get ; set ; }
    public float _timer { get ; set ; }
    public float hitAbleTime_Normalized;
    public float endHitableTime_Normalized;
    public AnimationClip _animationClip { get; set; }
    public bool _isExit { get => _timer >= _animationClip.length * _exitTime_Normalized ; set { } }
    public bool _isTransitionAble { get => _timer >= _transitionAbleTime_Nornalized * _animationClip.length ; set { } }

    public GunFuHitNodeLeaf(Player player,GunFuHitNodeScriptableObject gunFuNodeScriptableObject) : base(player)
    {
        this._transitionAbleTime_Nornalized = gunFuNodeScriptableObject.TransitionAbleTime_Normalized;
        this._exitTime_Normalized = gunFuNodeScriptableObject.ExitTime_Normalized;
        this.hitAbleTime_Normalized = gunFuNodeScriptableObject.HitAbleTime_Normalized;
        this.endHitableTime_Normalized = gunFuNodeScriptableObject.EndHitAbleTime_Normalized;
        this._animationClip = gunFuNodeScriptableObject.animationClip;
    }
    public override void Enter()
    {
        _timer = 0;
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

    public override void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= _transitionAbleTime_Nornalized* _animationClip.length)
            _isTransitionAble = true;
    }
}
