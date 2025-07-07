using System.Collections.Generic;
using System;
using UnityEngine;

public class GunFuHitNodeLeaf : PlayerStateNodeLeaf, IGunFuNode, INodeLeafTransitionAble
{
    public float _timer { get; set; }
    public IGunFuAble gunFuAble { get => player; set { } }
    public IGotGunFuAttackedAble gotGunFuAttackedAble { get => player.attackedAbleGunFu; set { } }
    public AnimationClip _animationClip { get => gunFuHitScriptableObject.animationClip_GunFuHits; set { } }
    public GunFuHitScriptableObject gunFuHitScriptableObject { get => this._gunFuHitScriptableObject; }
    private GunFuHitScriptableObject _gunFuHitScriptableObject { get; set; }
    public string stateName => _gunFuHitScriptableObject.gunFuHitStateName;
    public int hitCount { get; protected set; }
    private float hitDistance = 0.5f;
    private bool isWarping;
    private int curWarpKeyFrame;

    private Quaternion lookAtTarget => Quaternion.LookRotation(
        (gotGunFuAttackedAble._character.transform.position - gunFuAble._character.transform.position).normalized
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
        base.Enter();
    }
    public override void UpdateNode()
    {
        _timer += Time.deltaTime;

        if(_timer > _animationClip.length * gunFuHitScriptableObject.ExitTime_Normalized)
            isComplete = true;

        if (_timer > _animationClip.length * gunFuHitScriptableObject.TransitionAbleTime_Normalized)
            nodeLeafTransitionBehavior.TransitionAbleAll(this);

        if(_timer > _animationClip.length * gunFuHitScriptableObject.hitTimesNormalized[hitCount])
        {
            (gotGunFuAttackedAble._character._movementCompoent as IMotionImplusePushAble).AddForcePush
                ((gotGunFuAttackedAble._character.transform.position - gunFuAble._character.transform.position).normalized * gunFuHitScriptableObject.hitPush[hitCount]
                , IMotionImplusePushAble.PushMode.InstanlyIgnoreMomentum);
            gotGunFuAttackedAble.TakeGunFuAttacked(this,gunFuAble);
            hitCount++;
        }

        PullUpdate();
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
            t = Mathf.Clamp01(_timer / _animationClip.length * gunFuHitScriptableObject.warpKeyFrameNormalized[0]);
            if(t <1)
            {
                if (gunFuAble == null)
                    Debug.Log("gunFuAble == null");

                if (gunFuAble._character == null)
                    Debug.Log("gunFuAble._character == null");

                //warp
                if (isWarping)
                MovementWarper.WarpMovement(
                    gunFuAble._character.transform.position
                    , gunFuAble._character.transform.rotation
                    , gunFuAble._character._movementCompoent
                    , gotGunFuAttackedAble._character.transform.position + (gunFuAble._character.transform.position - gotGunFuAttackedAble._character.transform.position ).normalized * hitDistance
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
        if(_timer - (_animationClip.length * gunFuHitScriptableObject.warpKeyFrameNormalized[curWarpKeyFrame-1])
            < _animationClip.length * gunFuHitScriptableObject.warpKeyFrameNormalized[curWarpKeyFrame])
        {
            t = Mathf.Clamp01(_timer / _animationClip.length * gunFuHitScriptableObject.warpKeyFrameNormalized[0]);
            //warp
            if (isWarping)
                MovementWarper.WarpMovement(
                    gunFuAble._character.transform.position
                    , gunFuAble._character.transform.rotation
                    , gunFuAble._character._movementCompoent
                    , gotGunFuAttackedAble._character.transform.position + (gunFuAble._character.transform.position - gotGunFuAttackedAble._character.transform.position).normalized * hitDistance
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

    }
    public bool Transitioning()
    { 
        return nodeLeafTransitionBehavior.Transitioning(this); 
    }
    public void AddTransitionNode(INode node)
    {
        nodeLeafTransitionBehavior.AddTransistionNode(this, node);
    }
}
