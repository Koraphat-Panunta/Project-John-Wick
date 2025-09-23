using System.Collections.Generic;
using System;
using UnityEngine;

public class GunFuHitNodeLeaf : PlayerStateNodeLeaf, IGunFuNode, INodeLeafTransitionAble
{
    public float staggerHitDamage => gunFuHitScriptableObject.staggerHitDamage;
    public float _timer { get; set; }
    public IGunFuAble gunFuAble { get => player; set { } }
    public IGotGunFuAttackedAble gotGunFuAttackedAble { get ; set; }
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
    private List<IGotGunFuAttackedAble> gotAttackedAlready;

    public GunFuHitNodeLeaf(Player player, Func<bool> preCondition,GunFuHitScriptableObject gunFuHitScriptableObject) : base(player, preCondition)
    {
        this._gunFuHitScriptableObject = gunFuHitScriptableObject;
        transitionAbleNode = new Dictionary<INode, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
        this.gotAttackedAlready = new List<IGotGunFuAttackedAble>();
    }
    public override void Enter()
    {
        gotAttackedAlready.Clear();
        gotGunFuAttackedAble = player.attackedAbleGunFu;
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

        if (hitCount <= gunFuHitScriptableObject.hitTimes.Count - 1)
        {
            if (_timer >= (_animationClip.length * gunFuHitScriptableObject.hitTimes[hitCount].y) - lenghtOffset)
            {
                Debug.Log("HitCount = " + hitCount);
                gotAttackedAlready.Clear();
                hitCount++;
            }
            else if (_timer > (_animationClip.length * gunFuHitScriptableObject.hitTimes[hitCount].x) - lenghtOffset
            && _timer < (_animationClip.length * gunFuHitScriptableObject.hitTimes[hitCount].y) - lenghtOffset)
            {
                Attacking();
            }

        }
       

        PullUpdate();
        nodeLeafTransitionBehavior.TransitioningCheck(this);
        base.UpdateNode();
    }

    protected void Attacking()
    {
        Vector3 shperePos = player.transform.position
    + (player.transform.forward * gunFuHitScriptableObject.attackVolumeForward)
    + (player.transform.up * gunFuHitScriptableObject.attackVolumeUpward)
    + (player.transform.right * gunFuHitScriptableObject.attackVolumeRightward);

        Debug.Log("attacking");

        Debug.DrawLine(player.transform.position,shperePos,Color.green,0.5f);

        player._gunFuDetectTarget.CastDetectTargetInVolume(out List<IGotGunFuAttackedAble> targets, shperePos, gunFuHitScriptableObject.attackVolumeRaduis);

        if (targets.Count <= 0)
            return;

        for(int i = 0; i < targets.Count; i++)
        {
            Debug.Log("i = " + i);

            Debug.Log("target = " + targets[i] + "1");

            if (this.gotAttackedAlready.Contains(targets[i]))
                continue;

            Debug.Log("target = " + targets[i] + "2");

            if (targets[i]._isGotAttackedAble == false)
                continue;

            Debug.Log("target = " + targets[i] + "3");

            try 
            {
                Vector3 dir = Quaternion.AngleAxis(gunFuHitScriptableObject.hitPushRotationOffset[hitCount], Vector3.up) * (targets[i]._character.transform.position - gunFuAble._character.transform.position).normalized;
                (targets[i]._character._movementCompoent as IMotionImplusePushAble).AddForcePush
                    (dir * gunFuHitScriptableObject.hitPushForce[hitCount]
                    , IMotionImplusePushAble.PushMode.InstanlyIgnoreMomentum);
                curPhaseGunFuHit = GunFuPhaseHit.Attacking;
                targets[i].TakeGunFuAttacked(this, gunFuAble);
                this.gotAttackedAlready.Add(targets[i]);
                Debug.Log("PlayerNotufyHit");
                player.NotifyObserver(player, this);
            }
            catch
            {
                throw new Exception("i = "+i+" hitcount = "+hitCount);
            }

            
        }

        
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
        if(player._triggerHitedGunFu)
            return true;

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
                if(this.gotGunFuAttackedAble._isGotAttackedAble == false)
                    return;
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
                        , gotGunFuAttackedAble._character.transform.position + (gunFuAble._character.transform.position - gotGunFuAttackedAble._character.transform.position).normalized * hitDistance
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
