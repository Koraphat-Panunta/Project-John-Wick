using System.Collections.Generic;
using System;
using UnityEngine;

public class GunFuHitNodeLeaf : PlayerStateNodeLeaf, IGunFuNode, INodeLeafTransitionAble
{
    public float staggerHitDamage => gunFuHitScriptableObject.staggerHitDamage;
    public float _timer { get; set; }
    public IGunFuAble gunFuAble { get => player; set { } }
    public IGotGunFuAttackedAble gotGunExecutedAble { get => player.attackedAbleGunFu; set { } }
    public AnimationClip _animationClip { get => gunFuHitScriptableObject.animationClip_GunFuHits; set { } }
    public GunFuHitScriptableObject gunFuHitScriptableObject { get => this._gunFuHitScriptableObject; }
    private GunFuHitScriptableObject _gunFuHitScriptableObject { get; set; }
    public string _stateName => _gunFuHitScriptableObject.gunFuHitStateName;
    public int hitCount { get; protected set; }
    private float hitDistance = 0.7f;
    private bool isWarping;
    private int curWarpKeyFrame;
    private float lenghtOffset => _animationClip.length * gunFuHitScriptableObject.animationGunFuHitOffset;
    private Quaternion lookAtTarget => Quaternion.LookRotation(
        (gotGunExecutedAble._character.transform.position - gunFuAble._character.transform.position).normalized
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

    public GunFuHitNodeLeaf(Player player, Func<bool> preCondition,GunFuHitScriptableObject gunFuHitScriptableObject) : base(player, preCondition)
    {
        this._gunFuHitScriptableObject = gunFuHitScriptableObject;
        transitionAbleNode = new Dictionary<INode, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
    }
    public override void Enter()
    {
        curPhaseGunFuHit = GunFuPhaseHit.Enter;
        _timer = 0;
        hitCount = 0;
        isWarping = true;
        curWarpKeyFrame = 0;
        isComplete = false;
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        gunFuAble._character._movementCompoent.CancleMomentum();
        base.Enter();
    }
    public override void UpdateNode()
    {

        _timer += Time.deltaTime;

        if(_timer >(_animationClip.length * gunFuHitScriptableObject.ExitTime_Normalized) - lenghtOffset)
            isComplete = true;

        if (_timer > (_animationClip.length * gunFuHitScriptableObject.TransitionAbleTime_Normalized) - lenghtOffset)
            nodeLeafTransitionBehavior.TransitionAbleAll(this);

        if (hitCount <= gunFuHitScriptableObject.hitTimesNormalized.Count - 1)
        {
            if (_timer > (_animationClip.length * gunFuHitScriptableObject.hitTimesNormalized[hitCount])- lenghtOffset)
            {
                Vector3 dir = Quaternion.AngleAxis(gunFuHitScriptableObject.hitPushRotationOffset[hitCount], Vector3.up)  * (gotGunExecutedAble._character.transform.position - gunFuAble._character.transform.position).normalized;
                (gotGunExecutedAble._character._movementCompoent as IMotionImplusePushAble).AddForcePush
                    (dir * gunFuHitScriptableObject.hitPushForce[hitCount]
                    , IMotionImplusePushAble.PushMode.InstanlyIgnoreMomentum);
                curPhaseGunFuHit = GunFuPhaseHit.Attacking;
                gotGunExecutedAble.TakeGunFuAttacked(this, gunFuAble);

                   
                hitCount++;
                player.NotifyObserver(player,this);

            }
        }
       

        PullUpdate();
        nodeLeafTransitionBehavior.TransitioningCheck(this);
        base.UpdateNode();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override void Exit()
    {
        curPhaseGunFuHit = GunFuPhaseHit.Exit;
        base.Exit();
    }

    public override bool IsComplete()
    {
        return isComplete;
    }

    public override bool IsReset()
    {
        if(IsComplete())
            return true;

        if(player.isDead)
            return true;

        return false;
    }
    public void PullUpdate()
    {
        float t;

        
        if (curWarpKeyFrame == 0)
        {
            t = Mathf.Clamp01(_timer / ((_animationClip.length * gunFuHitScriptableObject.warpKeyFrameNormalized[0] 
                )- lenghtOffset));
            if(t <1)
            {
              
                //warp
                if (isWarping)
                MovementWarper.WarpMovement(
                    gunFuAble._character.transform.position
                    , gunFuAble._character.transform.rotation
                    , gunFuAble._character._movementCompoent
                    , gotGunExecutedAble._character.transform.position + (gunFuAble._character.transform.position - gotGunExecutedAble._character.transform.position ).normalized * hitDistance
                    , lookAtTarget
                    , t);
            }
            else
            {

                curWarpKeyFrame += 1;
                if (isWarping == true)
                    isWarping = false;
                else
                    isWarping = true;
            }
            return;
        }
        else // curKeyFrame > 0
        {
            if (curWarpKeyFrame >= gunFuHitScriptableObject.warpKeyFrameNormalized.Count - 1)
                return;


            t = Mathf.Clamp01((_timer - (_animationClip.length * gunFuHitScriptableObject.warpKeyFrameNormalized[curWarpKeyFrame -1] - _animationClip.length * gunFuHitScriptableObject.animationGunFuHitOffset))
                / (_animationClip.length * gunFuHitScriptableObject.warpKeyFrameNormalized[curWarpKeyFrame] - _animationClip.length * gunFuHitScriptableObject.warpKeyFrameNormalized[curWarpKeyFrame - 1]));
            //warp
            if (t < 1)
            {
                if (isWarping)
                {
                    MovementWarper.WarpMovement(
                        gunFuAble._character.transform.position
                        , gunFuAble._character.transform.rotation
                        , gunFuAble._character._movementCompoent
                        , gotGunExecutedAble._character.transform.position + (gunFuAble._character.transform.position - gotGunExecutedAble._character.transform.position).normalized * hitDistance
                        , lookAtTarget
                        , t);
                }
            }
            else
            {
                curWarpKeyFrame += 1;
                if (isWarping == true)
                    isWarping = false;
                else
                    isWarping = true;
            }
        }
       

    }
    public bool TransitioningCheck()
    { 
        return nodeLeafTransitionBehavior.TransitioningCheck(this); 
    }
    public void AddTransitionNode(INode node)
    {
        nodeLeafTransitionBehavior.AddTransistionNode(this, node);
    }
}
