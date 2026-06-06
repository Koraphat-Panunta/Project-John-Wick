using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingStateNodeLeaf : PlayerStateNodeLeaf,INodeLeafTransitionAble
{
    
    protected PlayerMovement playerMovement;

    public INodeManager nodeManager { get ; set ; }
    public Dictionary<INode, bool> transitionAbleNode { get ; set ; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    public float fallingVelocity { get; protected set; }

    public PlayerFallingStateNodeLeaf(Player player,PlayerStateNodeManager playerStateNodeManager,PlayerMovement playerMovement, Func<bool> preCondition) : base(player, preCondition)
    {

        this.nodeManager = playerStateNodeManager;
        this.playerMovement = playerMovement;

        this.transitionAbleNode = new Dictionary<INode, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();

    }

    public override bool IsReset()
    {
        if(this.player.isDead)
            return true;

        if(this.IsComplete())
            { return true; }

        return false;
    }

    public override bool IsComplete()
    {

        if(base.isComplete)
            return true;

        return false;
    }

    public override void Enter()
    {
        this.nodeLeafTransitionBehavior.TransitionAbleAll(this);
        base.isComplete = false;
        this.fallingVelocity = 0f;
        base.Enter();
    }
    public override void FixedUpdateNode()
    {
        if(this.player.inputMoveDir_World.magnitude >=0)
            this.playerMovement.UpdateMoveToDirWorld(player.inputMoveDir_World * this.player.StandMoveMaxSpeed * .25f, this.player.StandMoveAccelerate * .25f, MoveMode.MaintainMomentumDirection);
        else
            this.playerMovement.UpdateMoveToDirWorld(Vector3.zero,1, MoveMode.MaintainMomentumDirection);
        this.playerMovement.UpdateMovement();
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        if(this.playerMovement.characterController.isGrounded)
            this.isComplete = true;

        if(this.playerMovement.characterController.curVelocity.y *-1 > this.fallingVelocity )
            this.fallingVelocity = playerMovement.characterController.curVelocity.y * -1;

        //Debug.Log("Falling curVelocity = " + this.fallingVelocity);
        //Debug.Log("Falling velocity = " + this.playerMovement.characterController.velocityPhysicBased.y);

        this.TransitioningCheck();

        base.UpdateNode();
    }

    public bool TransitioningCheck() => this.nodeLeafTransitionBehavior.TransitioningCheck(this);
    public void AddTransitionNode(INode node) => this.nodeLeafTransitionBehavior.AddTransistionNode(this,node);
    
}
