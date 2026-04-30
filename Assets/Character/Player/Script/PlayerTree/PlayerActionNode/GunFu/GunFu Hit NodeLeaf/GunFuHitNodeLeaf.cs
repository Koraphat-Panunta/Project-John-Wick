using System.Collections.Generic;
using System;
using UnityEngine;

public class GunFuHitNodeLeaf : PlayerStateNodeLeaf
    , IGunFuNode
    ,IHPDamageVisitor
    ,IPostureDamageVisitor
    ,INodeLeafTransitionAble
{

    public float _postureDamageVisitor => this.gunFuHitScriptableObject.gunFuHitDetail[this.hitCount].postureHitDamage;
    public float _hPDamage => this.gunFuHitScriptableObject.gunFuHitDetail[this.hitCount].hpHitDamage;
    public float stuntingTime => this.gunFuHitScriptableObject.gunFuHitDetail[this.hitCount].stuntingTime; 
  
    public IGunFuAble gunFuAble { get => player; set { } }
    public IGotGunFuAttackedAble gotGunFuAttackedAble { get ; set; }
    public Vector3 approuchPosition { get => this.gotGunFuAttackedAble != null ? this.gotGunFuAttackedAble._character.transform.position : this._approuchPositionValue; }
    protected Vector3 _approuchPositionValue;

    public GunFuHitScriptableObject gunFuHitScriptableObject { get => this._gunFuHitScriptableObject; }
    private GunFuHitScriptableObject _gunFuHitScriptableObject { get; set; }
    public string _stateName => _gunFuHitScriptableObject.gunFuHitDetail[this.hitCount].gunFuHitStateName;
    public int hitCount { get; protected set; }
    private float hitDistance = 0.7f;
    private bool isWarping;
    protected bool isAttackingTime;

    protected AnimationTriggerEventPlayer animationTriggerEventPlayer { get; set; }
    protected AnimationTriggerAudioEventPlayer animationTriggerAudioEventPlayer { get; set; }

    private Quaternion lookAtTarget => Quaternion.LookRotation(
        (this.approuchPosition - gunFuAble._character.transform.position).normalized
        , Vector3.up);
    public enum GunFuPhaseHit
    {
        Enter,
        Attacking,
        Exit,
    }
    public GunFuPhaseHit curPhaseGunFuHit { get; protected set; }
    public INodeManager nodeManager { get => player.playerStateNodeManager; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get ; set ; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get;set; }
    private List<IGotGunFuAttackedAble> gotAttackedAlready;

    public GunFuHitNodeLeaf(Player player, Func<bool> preCondition,GunFuHitScriptableObject gunFuHitScriptableObject) : base(player, preCondition)
    {
        this._gunFuHitScriptableObject = gunFuHitScriptableObject;
        transitionAbleNode = new Dictionary<INode, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
        this.gotAttackedAlready = new List<IGotGunFuAttackedAble>();

        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(gunFuHitScriptableObject);

        this.animationTriggerEventPlayer.SubscribeEvent("OpenAttackingTime",this.OpenAttackingTime);
        this.animationTriggerEventPlayer.SubscribeEvent("CloseAttackingTime",this.CloseAttackingTime);
        this.animationTriggerEventPlayer.SubscribeEvent("BeginWarp", this.BeginWarp);
        this.animationTriggerEventPlayer.SubscribeEvent("NextHitContinue",this.NextHitContinue);
        this.animationTriggerEventPlayer.SubscribeEvent("TransitionAble",this.TransitionAble);

        this.animationTriggerAudioEventPlayer = new AnimationTriggerAudioEventPlayer(
            this._gunFuHitScriptableObject.clip
            ,this._gunFuHitScriptableObject.enterNormalizedTime
            ,this._gunFuHitScriptableObject.endNormalizedTime
            ,this._gunFuHitScriptableObject.audiotriggerEvents);
    }
    public override void Enter()
    {
        this.animationTriggerEventPlayer.Rewind();
        this.animationTriggerAudioEventPlayer.Rewind();

        gotAttackedAlready.Clear();
        this.gotGunFuAttackedAble = player.attackedAbleGunFu;
        this._approuchPositionValue = this.gunFuAble._character.transform.position + this.gunFuAble._character.transform.forward;
        curPhaseGunFuHit = GunFuPhaseHit.Enter;

        hitCount = 0;
        isComplete = false;
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        gunFuAble._character._movementCompoent.CancleMomentum();
        base.Enter();
    }
    public override void UpdateNode()
    {
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
        this.animationTriggerAudioEventPlayer.Update(Time.deltaTime, this.gunFuAble._character.transform.position);

        if(this.animationTriggerEventPlayer.IsPlayFinish())
            isComplete = true;

        if (this.isAttackingTime)
            this.Attacking();
       
        if(this.gotGunFuAttackedAble != null)
            nodeLeafTransitionBehavior.TransitioningCheck(this);
        base.UpdateNode();
    }

    public Vector3 hitDir { get; protected set; }

    protected void Attacking()
    {
        Vector3 shperePos = player.transform.position
    + (this.player.transform.forward * gunFuHitScriptableObject.gunFuHitDetail[this.hitCount].attackVolumeForward)
    + (this.player.transform.up * gunFuHitScriptableObject.gunFuHitDetail[this.hitCount].attackVolumeUpward)
    + (this.player.transform.right * gunFuHitScriptableObject.gunFuHitDetail[this.hitCount].attackVolumeRightward);

        //Debug.Log("attacking");

        //Debug.DrawLine(player.transform.position,shperePos,Color.green,0.5f);

        player._gunFuDetectTarget.CastDetectTargetInVolume(out List<IGotGunFuAttackedAble> targets, shperePos, gunFuHitScriptableObject.gunFuHitDetail[this.hitCount].attackVolumeRaduis);

        if (targets.Count <= 0)
            return;

        for(int i = 0; i < targets.Count; i++)
        {
            //Debug.Log("i = " + i);

            //Debug.Log("target = " + targets[i] + "1");

            if (this.gotAttackedAlready.Contains(targets[i]))
                continue;

            //Debug.Log("target = " + targets[i] + "2");

            if (targets[i]._isGotAttackedAble == false)
                continue;

            //Debug.Log("target = " + targets[i] + "3");

            try 
            {
                this.hitDir = (targets[i]._character.transform.position - gunFuAble._character.transform.position).normalized;
                this.hitDir = Quaternion.Euler(this.gunFuHitScriptableObject.gunFuHitDetail[this.hitCount].hitDirRotOffset) * this.hitDir;

                (targets[i]._character._movementCompoent as IMotionImplusePushAble).AddForcePushInstantly
                    (this.hitDir * this.gunFuHitScriptableObject.gunFuHitDetail[hitCount].hitPushForce
                    , IMotionImplusePushAble.PushMode.IgnoreMomentum);
                curPhaseGunFuHit = GunFuPhaseHit.Attacking;
                targets[i].TakeGunFuAttacked(this, gunFuAble);
                this.gotAttackedAlready.Add(targets[i]);
                //Debug.Log("PlayerNotufyHit");
                player.NotifyObserver(player, this);
            }
            catch
            {
                throw new Exception("i = "+i+" hitcount = "+hitCount);
            }

            
        }


    }

    protected void OpenAttackingTime()
    {
        this.isAttackingTime = true;
    }
    protected void CloseAttackingTime() 
    {
        this.isAttackingTime = false;
        gotAttackedAlready.Clear();

    }

    protected void NextHitContinue() => this.hitCount++;

    protected Vector3 enterWarpPos;

    protected Vector3 exitWarpPos;

    protected void BeginWarp() 
    {
        this.enterWarpPos = this.gunFuAble._character.transform.position;

        this.UpdateExitWarp();

         this.isWarping = true; 
    }
    private void UpdateExitWarp()
    {
        this.exitWarpPos = this.approuchPosition;
    }
    protected void EndWarp() 
    {
        this.isWarping = false; 
        this.player.enableRootMotion = true;
    }

    protected void TransitionAble()
    {
        this.nodeLeafTransitionBehavior.TransitionAbleAll(this);
    }

    public override void FixedUpdateNode()
    {

        WarpingUpdate();

        base.FixedUpdateNode();
    }

    public override void Exit()
    {
        curPhaseGunFuHit = GunFuPhaseHit.Exit;
        this.player.enableRootMotion = false;

        base.Exit();
    }

    public override bool IsComplete()
    {
        return isComplete;
    }

    public override bool IsReset()
    {
        if(player._triggerHitedGunFu)
            return true;

        if(IsComplete())
            return true;

        if(player.isDead)
            return true;

        return false;
    }
    public void WarpingUpdate()
    {
        if(this.isWarping == false)
            return;

        this.UpdateExitWarp();

        float t = this.animationTriggerEventPlayer.GetRemapNormalizedTimer
            (this._gunFuHitScriptableObject.gunFuHitDetail[this.hitCount].warpingTime.x,
            this._gunFuHitScriptableObject.gunFuHitDetail[this.hitCount].warpingTime.y
            );

        MovementWarper.WarpMovement(
                    this.gunFuAble._character.transform.position
                    , this.gunFuAble._character.transform.rotation
                    , this.gunFuAble._character._movementCompoent
                    , this.approuchPosition + (this.gunFuAble._character.transform.position - this.approuchPosition).normalized * this.hitDistance
                    , this.lookAtTarget
                    , this._gunFuHitScriptableObject.gunFuHitDetail[this.hitCount].warpingMovementCurve.Evaluate(t)
                    );

        if(t >= 1)
            this.EndWarp();


    }
    public bool TransitioningCheck()
    { 
        return nodeLeafTransitionBehavior.TransitioningCheck(this); 
    }
    public void AddTransitionNode(INode node)
    {
        nodeLeafTransitionBehavior.AddTransistionNode(this, node);
    }

    public void OnNotifyFeedBackVisitor(IDamageAble damageAble)
    {
        this.player.OnNotifyFeedBackVisitor(damageAble);
    }
}
