using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpinKickGunFuNodeLeaf : EnemyStateLeafNode, IGunFuNode
{
    public float _transitionAbleTime_Nornalized { get; set; }
    public float _timer { get; set; }
    public IGunFuAble gunFuAble { get => enemy; set { } }
    public IGunFuGotAttackedAble attackedAbleGunFu { get; set; }
    public AnimationClip _animationClip { get; set; }
    public override bool isComplete { get => base.isComplete; protected set => base.isComplete = value; }
    private EnemySpinKickScriptable _enemySpinKickScriptable { get; set; }

    private bool isAlreadyPush;
    private Dictionary<IGunFuGotAttackedAble, bool> alreadyHittarget;

    private Vector3 targetPosition;
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

        if(enemy.gunFuDetectTarget.CastDetect(out IGunFuGotAttackedAble target))
        {
            targetPosition = target.attackedPos;
            Debug.Log("targetPosition = target.attackedPos;");
        }
        else
        {
            targetPosition = enemy.transform.position + enemy.transform.forward;
            Debug.Log(" targetPosition = enemy.transform.position + enemy.transform.forward;");
        }
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
        if(_timer >= _enemySpinKickScriptable._hitTimeEnterNormalized*_enemySpinKickScriptable.animationClip.length
            && _timer < _enemySpinKickScriptable._hitTimeExitNormalized * _enemySpinKickScriptable.animationClip.length)
        {
            Vector3 castPos = enemy.transform.position + enemy.transform.forward*_enemySpinKickScriptable._distanceCastVolume + enemy.transform.up*_enemySpinKickScriptable._upperCastOffsetVolume;
            gunFuAble.gunFuDetectTarget.CastDetectTargetInVolume(out List<IGunFuGotAttackedAble> targets, castPos,_enemySpinKickScriptable._raduisSphereVolume);

            if(targets.Count >0)
            targets.ForEach(target => 
            {
                if (alreadyHittarget.ContainsKey(target) == false)
                {
                    alreadyHittarget.Add(target, true);
                    target.TakeGunFuAttacked(this, enemy);
                    if(target._movementCompoent is IMotionImplusePushAble motionImplusePushAble)
                    {
                        Vector3 dir = target.attackedPos - enemy.transform.position;
                        motionImplusePushAble.AddForcePush(dir * _enemySpinKickScriptable._targetPushingForce, IMotionImplusePushAble.PushMode.InstanlyIgnoreMomentum);
                    }
                    enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuAttack);
                }
            }
            );
        }

        if(_timer < _enemySpinKickScriptable._stopRotatingTimeNormalized * _enemySpinKickScriptable.animationClip.length)
        {
            Vector3 dir = targetPosition - enemy.transform.position;

            enemy.enemyMovement.RotateToDirWorld(dir.normalized, _enemySpinKickScriptable._spicKickRotateSpeed);
        }


        if(_timer >= _enemySpinKickScriptable._pushForwardTimeNormalized*_enemySpinKickScriptable.animationClip.length && isAlreadyPush == false)//Push Enemy toward
        {
            enemy.enemyMovement.AddForcePush(enemy.transform.forward*_enemySpinKickScriptable._pushSelfTowardForce, IMotionImplusePushAble.PushMode.InstanlyMaintainMomentum);
            isAlreadyPush = true;
        }
        else if (isAlreadyPush == false && _timer < _enemySpinKickScriptable._pushForwardTimeNormalized * _enemySpinKickScriptable.animationClip.length)
        {    
            enemy.enemyMovement.MoveToDirWorld(Vector3.zero, _enemySpinKickScriptable._stopForceBeginStance, _enemySpinKickScriptable._stopForceBeginStance, IMovementCompoent.MoveMode.MaintainMomentum);
        }

        if (_timer >= _enemySpinKickScriptable._onGroundTimeNormalized * _enemySpinKickScriptable.animationClip.length)
        {
            enemy.enemyMovement.MoveToDirWorld(Vector3.zero, _enemySpinKickScriptable._stopingForceOnGround, _enemySpinKickScriptable._stopingForceOnGround,IMovementCompoent.MoveMode.MaintainMomentum);
        }
        
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

        return IsComplete();
    }

    public override void UpdateNode()
    {
        _timer += Time.deltaTime;
        if(_timer >= _enemySpinKickScriptable.animationClip.length)
            isComplete = true;
        base.UpdateNode();
    }
}
[CreateAssetMenu(fileName = "EnemySpinKickScriptable", menuName = "ScriptableObjects/EnemySpinKick")]
public class EnemySpinKickScriptable : ScriptableObject
{
    public AnimationClip animationClip;

    [Range(0,1)]
    public float _pushForwardTimeNormalized;
    [Range(0, 1)]
    public float _hitTimeEnterNormalized;
    [Range(0, 1)]
    public float _hitTimeExitNormalized;
    [Range(0, 1)]
    public float _onGroundTimeNormalized;
    [Range(0,1)]
    public float _stopRotatingTimeNormalized;

    [Range(0, 50)]
    public float _pushSelfTowardForce;

    [Range(0, 100)]
    public float _spicKickRotateSpeed;

    [Range(0, 10)]
    public float _distanceCastVolume;

    [Range(0, 10)]
    public float _upperCastOffsetVolume;

    [Range(0, 10)]
    public float _raduisSphereVolume;

    [Range(0,100)]
    public float _stopingForceOnGround;

    [Range(0, 100)]
    public float _stopForceBeginStance;

    [Range(0, 100)]
    public float _targetPushingForce;
}
