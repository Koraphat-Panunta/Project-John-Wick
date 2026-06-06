using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationBaseState : PlayerStateNodeLeaf, INodeLeafTransitionAble
{
    public INodeManager nodeManager { get => player.playerStateNodeManager; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    protected AnimationTriggerEventPlayer animationTriggerEventPlayer;
    protected virtual bool EnableRootMotionOnEnter => true;

    private readonly Func<bool> _resetCondition;

    public PlayerAnimationBaseState(Player player, AnimationTriggerEventSCRP animationSCRP, Func<bool> preCondition, Func<bool> resetCondition = null)
        : base(player, preCondition)
    {
        _resetCondition = resetCondition;
        transitionAbleNode = new Dictionary<INode, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
        animationTriggerEventPlayer = new AnimationTriggerEventPlayer(animationSCRP);
    }

    public override void Enter()
    {
        this.player._movementCompoent.CancleMomentum();
        isComplete = false;
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        animationTriggerEventPlayer.Rewind();
        if (EnableRootMotionOnEnter) player.enableRootMotion = true;
        base.Enter();
    }

    public override void Exit()
    {
        player.enableRootMotion = false;
        base.Exit();
    }

    public override void UpdateNode()
    {
        animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
        if (animationTriggerEventPlayer.IsPlayFinish())
        {
            isComplete = true;
            nodeLeafTransitionBehavior.TransitionAbleAll(this);
        }
        TransitioningCheck();
        base.UpdateNode();
    }

    public override bool IsComplete() => isComplete;

    public override bool IsReset()
    {
        if (_resetCondition != null && _resetCondition()) return true;
        return IsComplete();
    }

    public bool TransitioningCheck() => nodeLeafTransitionBehavior.TransitioningCheck(this);
    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this, node);
}
