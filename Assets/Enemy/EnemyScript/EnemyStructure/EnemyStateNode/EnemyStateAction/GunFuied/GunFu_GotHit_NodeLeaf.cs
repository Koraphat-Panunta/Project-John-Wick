using System;
using UnityEngine;

public abstract class GunFu_GotHit_NodeLeaf : EnemyStateLeafNode,IGunFuAttackedAbleNode
{
    protected Animator animator;
    protected string stateName;

    public IGunFuAble gunFuAble;
    public GunFu_GotHit_NodeLeaf(Enemy enemy,Func<bool> preCondition,GunFu_GotHit_ScriptableObject gunFu_GotHit_ScriptableObject) : base(enemy,preCondition)
    {
        _exitTime_Normalized = gunFu_GotHit_ScriptableObject.ExitTime_Normalized;
        _animationClip = gunFu_GotHit_ScriptableObject.AnimationClip;
        this.animator = enemy.animator;
        stateName = gunFu_GotHit_ScriptableObject.StateName;
    }
    public override void Enter()
    {
        _timer = 0;


        base.Enter();
    }
    public override void Exit()
    {
        enemy.curGotAttackedGunFuNode = null;
        base.Exit();
    }
    public override void UpdateNode()
    {
        _timer += Time.deltaTime;

        Debug.Log("Time " + _timer + " >= _animationClip.length*_exitTime_Nor " + _animationClip.length * _exitTime_Normalized);
        Debug.Log("isComplete = " + isComplete);

        if(_timer >= _animationClip.length*_exitTime_Normalized)
            isComplete = true;

        base.UpdateNode();
    }

    public override bool IsReset()
    {
        if(IsComplete())
            return true;

        if(enemy.isDead)
            return true;

        if(enemy._isPainTrigger
            ||enemy._triggerHitedGunFu)
            return true;

        return false;
    }

    public float _exitTime_Normalized { get; set; }
    public float _timer { get; set; }
    public AnimationClip _animationClip { get; set; }
}
