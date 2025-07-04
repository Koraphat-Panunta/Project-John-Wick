using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerGunFuHitNodeLeaf : PlayerStateNodeLeaf ,IGunFuNode,INodeLeafTransitionAble
{
    public float _timer { get; set; }
    public float _transitionAbleTime_Nornalized { get => gunFuNodeScriptableObject.TransitionAbleTime_Normalized; set { } }
    public float _exitTime_Normalized { get => gunFuNodeScriptableObject.ExitTime_Normalized; set { } }
    public float hitAbleTime_Normalized => gunFuNodeScriptableObject.HitAbleTime_Normalized;
    public float endHitableTime_Normalized => gunFuNodeScriptableObject.EndHitAbleTime_Normalized;
    public AnimationClip _animationClip { get; set; }

    public IGunFuGotAttackedAble attackedAbleGunFu { get; set; }
    public IGunFuAble gunFuAble { get; set; }
    public INodeManager nodeManager { get => base.player.playerStateNodeManager ; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get ; set ; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }
    protected GunFuHitNodeScriptableObject gunFuNodeScriptableObject;
    public Transform targetAdjustTransform => gunFuAble._targetAdjustTranform;

    public enum GunFuHitPhase
    {
        Enter,
        Hit,
        Exit
    }
    public GunFuHitPhase curGunFuHitPhase { get; protected set; }

    public PlayerGunFuHitNodeLeaf(Player player,Func<bool> preCondition,GunFuHitNodeScriptableObject gunFuNodeScriptableObject) : base(player,preCondition)
    {
        this.gunFuNodeScriptableObject = gunFuNodeScriptableObject;
        this._animationClip = gunFuNodeScriptableObject.animationClip;

        gunFuAble = player as IGunFuAble;
        transitionAbleNode = new Dictionary<INode, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
    }
    public override void Enter()
    {
        this.attackedAbleGunFu = gunFuAble.attackedAbleGunFu;
        targetEnterPos = attackedAbleGunFu._gunFuAttackedAble.position;

        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        _timer = 0;

        gunFuAble._triggerGunFu = false;
        gunFuAble.triggerGunFuBufferTime = 2;

        (player.playerMovement as IMovementCompoent).CancleMomentum();

        curGunFuHitPhase = GunFuHitPhase.Enter;
        base.Enter();
    }

    public override void Exit()
    {
        _timer = 0;
        (player.playerMovement as IMovementCompoent).CancleMomentum();
        curGunFuHitPhase -= GunFuHitPhase.Exit;
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        if (_timer <= _animationClip.length * hitAbleTime_Normalized)
        {
            Vector3 warpPos = targetAdjustTransform.position + targetAdjustTransform.forward * 0.2f; 
            attackedAbleGunFu._gunFuAttackedAble.position = Vector3.Lerp(
                      attackedAbleGunFu._gunFuAttackedAble.position,
                     warpPos,
                      _timer / (_animationClip.length * hitAbleTime_Normalized)
                      );
            curGunFuHitPhase = GunFuHitPhase.Hit;
            player.NotifyObserver(player, this);
            attackedAbleGunFu._gunFuAttackedAble.rotation = Quaternion.Lerp(attackedAbleGunFu._gunFuAttackedAble.rotation
                , Quaternion.LookRotation(targetAdjustTransform.forward * -1, Vector3.up)
                , _timer / (_animationClip.length * hitAbleTime_Normalized));
        }
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        Transitioning();

        _timer += Time.deltaTime;
        if (_timer >= _transitionAbleTime_Nornalized * _animationClip.length)
            nodeLeafTransitionBehavior.TransitionAbleAll(this);
            

        if(_timer >= _exitTime_Normalized * _animationClip.length)
            isComplete = true;

        base.UpdateNode();
    }

    RotateObjectToward rotateObjectToward = new RotateObjectToward();

    private Vector3 targetEnterPos;
    protected void LerpingToTargetPos()
    {
        Vector3 targetPos = targetEnterPos;
        //Vector3 offset = (gunFuAble._gunFuUserTransform.position - targetPos).normalized;
        //offset = new Vector3(offset.x,0,offset.z).normalized * 0.35f;

        if (Vector3.Distance(targetPos, player.transform.position) > 0.6f)
        {
            player.playerMovement.RotateToDirWorld(attackedAbleGunFu._gunFuAttackedAble.position - player.transform.position, 15);
            player.playerMovement.SnapingMovement(targetPos, Vector3.zero, 300 * Time.deltaTime);
        }
    }

    public bool Transitioning() => nodeLeafTransitionBehavior.Transitioning(this);
    public void AddTransitionNode(INode nodeLeaf) => nodeLeafTransitionBehavior.AddTransistionNode(this, nodeLeaf);
   
}
