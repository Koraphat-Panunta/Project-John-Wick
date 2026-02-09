using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProneStateNodeLeaf : PlayerStateNodeLeaf,INodeLeafTransitionAble
{
    protected PlayerMovement playerMovement => this.player.playerMovement;

    public INodeManager nodeManager { get; set; }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    private float timer;
    private float stayTime = 1;

    public PlayerProneStateNodeLeaf(Player player,PlayerStateNodeManager playerStateNodeManager, Func<bool> preCondition) : base(player, preCondition)
    {
        this.nodeManager = playerStateNodeManager;
        this.transitionAbleNode = new Dictionary<INode, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
    }
    public override void Enter()
    {
        this.nodeLeafTransitionBehavior.TransitionAbleAll(this);
        this.timer = 0;
        base.Enter();
    }
    public override void Exit()
    {
        this.player.playerStance = Stance.stand;
        base.Exit();
    }
    public override void UpdateNode()
    {
        
        if(this.timer < stayTime)
        this.timer += Time.deltaTime;
        else
            this.TransitioningCheck();

        base.UpdateNode();
    }

    public override void FixedUpdateNode()
    {
        this.playerMovement.UpdateMoveToDirWorld(Vector3.zero, this.player.breakDecelerate , MoveMode.MaintainMomentumDirection);
        base.FixedUpdateNode();
    }
    public override bool IsReset()
    {
        if(this.player.isDead)
            return true; 

        return false;
    }

    public bool TransitioningCheck() => this.nodeLeafTransitionBehavior.TransitioningCheck(this);

    public void AddTransitionNode(INode node) => this.nodeLeafTransitionBehavior.AddTransistionNode(this, node);
   
}
