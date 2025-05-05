using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpinKickGunFuNodeLeaf : EnemyStateLeafNode, IGunFuNode
{
    public float _transitionAbleTime_Nornalized { get; set; }
    public float _timer { get; set; }
    public IGunFuAble gunFuAble { get => enemy; set { } }
    public IGunFuGotAttackedAble attackedAbleGunFu { get; set; }
    public AnimationClip _animationClip { get => _enemySpinKickScriptable.animationClip; set => _enemySpinKickScriptable.animationClip = value; }
    public override bool isComplete { get => base.isComplete; protected set => base.isComplete = value; }
    private EnemySpinKickScriptable _enemySpinKickScriptable { get; set; }

    private bool isAlreadyPush;
    private Dictionary<IGunFuGotAttackedAble, bool> alreadyHittarget;

    private Vector3 targetPosition => enemy.targetKnewPos;
    public EnemySpinKickGunFuNodeLeaf(EnemySpinKickScriptable enemySpinKickScriptable,Enemy enemy, Func<bool> preCondition) : base(enemy, preCondition)
    {
        this._enemySpinKickScriptable = enemySpinKickScriptable;
        alreadyHittarget = new Dictionary<IGunFuGotAttackedAble, bool> ();
    }

  
    public override void Enter()
    {
        _timer = 0;
        isComplete = false;
        isAlreadyPush = false;

       
        alreadyHittarget.Clear();
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuEnter);
        base.Enter();
    }

    public override void Exit()
    {
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuExit);
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
       
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return base.IsComplete();
    }

    public override bool IsReset()
    {
        if(enemy.isDead || enemy._posture <= 0)
            return true;

        if(enemy._triggerHitedGunFu)
            return true;

        if(enemy._isPainTrigger)
            return true;

        if(_timer < _animationClip.length*_enemySpinKickScriptable._pushForwardTimeNormalized
            &&
            (
            enemy._isPainTrigger
            ))
            return true;


        return IsComplete();
    }

    public override void UpdateNode()
    {
        _timer += Time.deltaTime;
        if(_timer >= _enemySpinKickScriptable.animationClip.length)
            isComplete = true;

        if (_timer >= _enemySpinKickScriptable._hitTimeEnterNormalized * _enemySpinKickScriptable.animationClip.length
           && _timer < _enemySpinKickScriptable._hitTimeExitNormalized * _enemySpinKickScriptable.animationClip.length)
        {
            Vector3 castPos = enemy.transform.position + enemy.transform.forward * _enemySpinKickScriptable._distanceCastVolume + enemy.transform.up * _enemySpinKickScriptable._upperCastOffsetVolume;
            gunFuAble.gunFuDetectTarget.CastDetectTargetInVolume(out List<IGunFuGotAttackedAble> targets, castPos, _enemySpinKickScriptable._raduisSphereVolume);

            if (targets.Count > 0)
                targets.ForEach(target =>
                {
                    if (alreadyHittarget.ContainsKey(target) == false)
                    {
                        alreadyHittarget.Add(target, true);
                        target.TakeGunFuAttacked(this, enemy);
                        if (target._movementCompoent is IMotionImplusePushAble motionImplusePushAble)
                        {
                            Vector3 dir = target._gunFuAttackedAble.position - enemy.transform.position;
                            motionImplusePushAble.AddForcePush(dir.normalized * _enemySpinKickScriptable._targetPushingForce, IMotionImplusePushAble.PushMode.InstanlyIgnoreMomentum);
                        }
                        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuAttack);
                    }
                }
                );
        }

        if (_timer < _enemySpinKickScriptable._stopRotatingTimeNormalized * _enemySpinKickScriptable.animationClip.length)
        {
            Vector3 dir = targetPosition - enemy.transform.position;

            enemy.enemyMovement.RotateToDirWorld(dir.normalized, _enemySpinKickScriptable._spicKickRotateSpeed);
        }


        if (_timer >= _enemySpinKickScriptable._pushForwardTimeNormalized * _enemySpinKickScriptable.animationClip.length && isAlreadyPush == false)//Push Enemy toward
        {
            enemy.enemyMovement.AddForcePush(enemy.transform.forward * _enemySpinKickScriptable._pushSelfTowardForce, IMotionImplusePushAble.PushMode.InstanlyIgnoreMomentum);
            isAlreadyPush = true;
        }
        else if (isAlreadyPush == false && _timer < _enemySpinKickScriptable._pushForwardTimeNormalized * _enemySpinKickScriptable.animationClip.length)
        {
            enemy.enemyMovement.MoveToDirWorld(Vector3.zero, _enemySpinKickScriptable._stopForceBeginStance, _enemySpinKickScriptable._stopForceBeginStance, IMovementCompoent.MoveMode.MaintainMomentum);
        }

        if (_timer >= _enemySpinKickScriptable._onGroundTimeNormalized * _enemySpinKickScriptable.animationClip.length)
        {
            enemy.enemyMovement.MoveToDirWorld(Vector3.zero, _enemySpinKickScriptable._stopingForceOnGround, _enemySpinKickScriptable._stopingForceOnGround, IMovementCompoent.MoveMode.MaintainMomentum);
        }

        base.UpdateNode();
    }
}

