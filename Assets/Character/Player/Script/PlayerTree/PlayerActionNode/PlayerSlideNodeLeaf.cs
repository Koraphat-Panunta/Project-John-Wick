using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideNodeLeaf : PlayerStateNodeLeaf, INodeLeafTransitionAble
{
    private PlayerMovement playerMovement => player._movementCompoent as PlayerMovement;

    public INodeManager nodeManager { get; set; }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    private readonly UnGripMovement _slideSCRP;
    private readonly AnimationTriggerEventPlayer _animTriggerPlayer;
    private readonly float _animDuration;

    private Vector3 _slideDir;
    private float _elapsed;
    private float _normalizedTime;

    private float breakDecelerate => this._slideSCRP.gripCurve.Evaluate(this._animTriggerPlayer.timerNormalized);
    public float gripWeight => _slideSCRP.gripCurve.Evaluate(_normalizedTime);

    public PlayerSlideNodeLeaf(Player player, PlayerStateNodeManager manager, UnGripMovement slideSCRP, Func<bool> preCondition)
        : base(player, preCondition)
    {
        _slideSCRP = slideSCRP;
        _animTriggerPlayer = new AnimationTriggerEventPlayer(slideSCRP.animationTriggerEvent);
        _animDuration = slideSCRP.animationTriggerEvent.clip.length;
        this.nodeManager = manager;
        this.transitionAbleNode = new Dictionary<INode, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();

        this._animTriggerPlayer.SubscribeEvent("TransitionAble", this.TransitionAble);

    }

    public override void Enter()
    {
        this.nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);

        _slideDir = playerMovement.curMoveVelocity_World.magnitude > 0
            ? playerMovement.curMoveVelocity_World.normalized
            : player.inputMoveDir_World;

        this.playerMovement.AddForcePushInstantly(_slideDir.normalized, IMotionImplusePushAble.PushMode.MaintainMomentum);

        _elapsed = 0f;
        _normalizedTime = 0f;
        _animTriggerPlayer.Rewind();
        base.Enter();
    }

    public override void UpdateNode()
    {
        _animTriggerPlayer.UpdatePlay(Time.deltaTime);
        _elapsed += Time.deltaTime;
        _normalizedTime = Mathf.Clamp01(_elapsed / _animDuration);
        if (_animTriggerPlayer.IsPlayFinish())
            isComplete = true;
        this.TransitioningCheck();
        base.UpdateNode();
    }

    public override void FixedUpdateNode()
    {
        playerMovement.UpdateMoveToDirWorld(Vector3.zero, this.breakDecelerate, MoveMode.IgnoreMomentumDirection);
        playerMovement.SetRotateToDirWorldSlerp(_slideDir, 1f);
        base.FixedUpdateNode();
    }

    public override void Exit() => base.Exit();
    
    

    public override bool IsReset()
    {
        if (this.IsComplete())
            return true;

        if (player.isDead) 
            return true;

        return false;
    }
    private void TransitionAble() => this.nodeLeafTransitionBehavior.TransitionAbleAll(this);
    public bool TransitioningCheck() => this.nodeLeafTransitionBehavior.TransitioningCheck(this);
    public void AddTransitionNode(INode node) => this.nodeLeafTransitionBehavior.AddTransistionNode(this, node);
}
