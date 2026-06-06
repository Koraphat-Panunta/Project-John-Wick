using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_OCM_Hit_NodeLeaf : EnemyStateLeafNode, I_OCM_Node,IHPDamageVisitor
{
   
    public OCM_ManaverStateName ocm_ManaverState;
    public I_OCM_Attack_Able gunFuAble { get => enemy; set { } }
    public I_Got_OCM_Attacked_Able gotGunFuAttackedAble { get; set; }

    public override bool isComplete { get => base.isComplete; protected set => base.isComplete = value; }

    private GunFuHitScriptableObject _enemyOCM_Hit_Scriptable { get; set; }
    public AnimationTriggerEventPlayer animationTriggerEventPlayer { get; set; }

    private Dictionary<I_Got_OCM_Attacked_Able, bool> alreadyHittarget;

    private Vector3 targetPosition => this.enemy.targetKnowPos;
    public string _stateName { get => this._enemyOCM_Hit_Scriptable.gunFuHitDetail[0].gunFuHitStateName; }

  
    public MeleeAttackingPhase curPhaseGunFuHit { get; protected set; }

    public NodePhase _curPhase => this.curstate;

    public float _hPDamage => this._enemyOCM_Hit_Scriptable.gunFuHitDetail[0].hpHitDamage;

    protected float beginMoveNormalizedTime;
    protected float finishMoveWarpPos;
    public bool triggerAttack;
    public Enemy_OCM_Hit_NodeLeaf(
        GunFuHitScriptableObject enemyOCM_Hit_Scriptable
        ,Enemy enemy
        ,OCM_ManaverStateName oCM_ManaverState
        , Func<bool> preCondition) : base(enemy, preCondition)
    {
        this.ocm_ManaverState = oCM_ManaverState;
        this._enemyOCM_Hit_Scriptable = enemyOCM_Hit_Scriptable;
        alreadyHittarget = new Dictionary<I_Got_OCM_Attacked_Able, bool> ();
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(this._enemyOCM_Hit_Scriptable);

        this.animationTriggerEventPlayer.SubscribeEvent(MeleeAttackingPhase.Anticipate.ToString(), this.Anticipate);
        this.animationTriggerEventPlayer.SubscribeEvent(MeleeAttackingPhase.PreAttack.ToString(), this.PreAttack);
        this.animationTriggerEventPlayer.SubscribeEvent(MeleeAttackingPhase.Attacking.ToString(), this.Attacking);
        this.animationTriggerEventPlayer.SubscribeEvent(MeleeAttackingPhase.PostAttack.ToString(), this.PostAttack);

        this.animationTriggerEventPlayer.GetNormalizedTimeFromStateName(MeleeAttackingPhase.Anticipate.ToString(),out this.beginMoveNormalizedTime);
        this.animationTriggerEventPlayer.GetNormalizedTimeFromStateName(MeleeAttackingPhase.PreAttack.ToString(),out this.finishMoveWarpPos);
    }


    public override void Enter()
    {
        Vector3 targetDir = (this.targetPosition - this.enemy._movementCompoent.curPosition);
        this.enemy._movementCompoent.CancleMomentum();

        this.enemy._movementCompoent.SetRotateToDirWorldSlerp(targetDir,1);

        this.animationTriggerEventPlayer.Rewind();
        isComplete = false;     
        this.alreadyHittarget.Clear();
        curPhaseGunFuHit = MeleeAttackingPhase.None;
        this.triggerAttack = false;
        base.Enter();
    }

    public override void Exit()
    {
        this.enemy.enableRootMotion = false;
        curPhaseGunFuHit = MeleeAttackingPhase.None;
        enemy.NotifyObserver(enemy, this);
        base.Exit();
    }

    public override void FixedUpdateNode()
    {

        switch (this.curPhaseGunFuHit)
        {
            case MeleeAttackingPhase.Anticipate:
                {
                    this.MoveToTargetPos();
                    break;
                }
            case MeleeAttackingPhase.PreAttack:
                {
                    break;
                }
            case MeleeAttackingPhase.Attacking:
                {
                    Vector3 castPos = this.enemy.transform.position
                + (this.enemy.transform.forward * this._enemyOCM_Hit_Scriptable.gunFuHitDetail[0].attackVolumeForward)
                + (this.enemy.transform.up * this._enemyOCM_Hit_Scriptable.gunFuHitDetail[0].attackVolumeUpward)
                + (this.enemy.transform.right * this._enemyOCM_Hit_Scriptable.gunFuHitDetail[0].attackVolumeRightward);

                    DrawSphereGizmo(castPos, this._enemyOCM_Hit_Scriptable.gunFuHitDetail[0].attackVolumeRaduis, Color.red, 5f);

                    this.gunFuAble._gunFuDetectTarget.CastDetectTargetInVolume
                        (out List<I_Got_OCM_Attacked_Able> targets
                        , castPos
                        , this._enemyOCM_Hit_Scriptable.gunFuHitDetail[0].attackVolumeRaduis
                        , LayerMask.GetMask("Player") | LayerMask.GetMask("Enemy"));

                    

                    if (targets == null)
                        return;

                    if (targets.Count > 0)
                        targets.ForEach(target =>
                        {
                            Debug.Log("targets = " + target._character.gameObject);

                            if (this.alreadyHittarget.ContainsKey(target) == false)
                            {
                                this.alreadyHittarget.Add(target, true);
                                target.TakeGunFuAttacked(this, enemy,true);
                                this.enemy.NotifyObserver(enemy, this);
                            }
                        }
                        );
                    break;
                }
            case MeleeAttackingPhase.PostAttack:
                {
                    break;
                }
        }

        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return base.IsComplete();
    }

    public override bool IsReset()
    {
        if(this.enemy.isDead || enemy._posture <= 0)
            return true;

        if(this.enemy._isPainTrigger)
            return true;

        if(this.curPhaseGunFuHit >= MeleeAttackingPhase.PreAttack
            && this.enemy._isPainTrigger)
            return true;

        return IsComplete();
    }



    public override void UpdateNode()
    {
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);



        if(this.curPhaseGunFuHit == MeleeAttackingPhase.Anticipate
            || this.curPhaseGunFuHit == MeleeAttackingPhase.PreAttack)
            this.RotateUpdate();

        if (this.animationTriggerEventPlayer.IsPlayFinish())
            isComplete = true;

        base.UpdateNode();
    }
    private void RotateUpdate()
    {

        Vector3 targetDir = (this.targetPosition - this.enemy._movementCompoent.curPosition);

        Debug.DrawRay(this.enemy._movementCompoent.curPosition, targetDir, Color.red, 5);


        this.enemy._movementCompoent.SetRotateToDirWorld(targetDir, this._enemyOCM_Hit_Scriptable.gunFuHitDetail[0].attackRotateVelocity);
    }
    private void MoveToTargetPos()
    {
        Vector3 targetDir = (this.targetPosition - this.enemy._movementCompoent.curPosition);
        targetDir = new Vector3(targetDir.x, 0, targetDir.z).normalized;

        float t = this.animationTriggerEventPlayer.GetRemapNormalizedTimer(this.beginMoveNormalizedTime, this.finishMoveWarpPos);

        if (Vector3.Distance(this.targetPosition, this.enemy._movementCompoent.curPosition) > this._enemyOCM_Hit_Scriptable.gunFuHitDetail[0].attackRange)
            this.enemy._movementCompoent.Move(
                targetDir * this._enemyOCM_Hit_Scriptable.gunFuHitDetail[0].warpingMovementCurve.Evaluate(t) * this._enemyOCM_Hit_Scriptable.gunFuHitDetail[0].attackMoveVelocity * Time.deltaTime
                );
    }

    public void Anticipate()
    {
        this.curPhaseGunFuHit = MeleeAttackingPhase.Anticipate;
        this.enemy.enableRootMotion = false;
    }
    public void PreAttack() 
    { 
        this.curPhaseGunFuHit = MeleeAttackingPhase.PreAttack; 
        this.enemy.enableRootMotion = true;
    }
    public void Attacking()
    {
        this.curPhaseGunFuHit = MeleeAttackingPhase.Attacking;



    }
    public void PostAttack() => this.curPhaseGunFuHit = MeleeAttackingPhase.PostAttack;
    public void TriggerAttack() => this.triggerAttack = true;
    public void OnNotifyFeedBackVisitor(IDamageAble damageAble)
    {

    }

    private static void DrawSphereGizmo(Vector3 center, float radius, Color color, float duration)
    {
        const int segments = 16;
        const float step = 2f * Mathf.PI / segments;
        for (int i = 0; i < segments; i++)
        {
            float a0 = i * step;
            float a1 = (i + 1) * step;
            float cos0 = Mathf.Cos(a0) * radius, sin0 = Mathf.Sin(a0) * radius;
            float cos1 = Mathf.Cos(a1) * radius, sin1 = Mathf.Sin(a1) * radius;
            Debug.DrawLine(center + new Vector3(cos0, sin0, 0), center + new Vector3(cos1, sin1, 0), color, duration);
            Debug.DrawLine(center + new Vector3(cos0, 0, sin0), center + new Vector3(cos1, 0, sin1), color, duration);
            Debug.DrawLine(center + new Vector3(0, cos0, sin0), center + new Vector3(0, cos1, sin1), color, duration);
        }
    }
}

