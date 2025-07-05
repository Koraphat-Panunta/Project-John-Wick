using System;
using UnityEngine;

public abstract class GunFu_GotHit_NodeLeaf : EnemyStateLeafNode,IGotGunFuAttackNode
{
    protected Animator animator;
    protected string stateName;

    public IGunFuAble gunFuAble;

    float forcePush => enemy.hitedForcePush;
    float forceStop => enemy.hitedForceStop;
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
        Vector3 hitPushDir = (enemy.transform.position - enemy.gunFuAbleAttacker._gunFuUserTransform.position).normalized;
        (enemy._movementCompoent as EnemyMovement).AddForcePush(hitPushDir*forcePush, IMotionImplusePushAble.PushMode.InstanlyIgnoreMomentum);

        base.Enter();
    }
    public override void Exit()
    {
        enemy.curAttackerGunFuNode = null;
        base.Exit();
    }
    public override void UpdateNode()
    {
        _timer += Time.deltaTime;

        if(_timer >= _animationClip.length*_exitTime_Normalized)
            isComplete = true;

        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        enemy._movementCompoent.MoveToDirWorld(Vector3.zero, forceStop, forceStop, MoveMode.MaintainMomentum);    
        base.FixedUpdateNode();
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
