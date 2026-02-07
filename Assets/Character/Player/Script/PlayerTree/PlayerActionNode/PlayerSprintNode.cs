using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintNode : PlayerStateNodeLeaf,INodeLeafTransitionAble
{
    private PlayerMovement playerMovement => player._movementCompoent as PlayerMovement;
    private Vector3 sprintDir;

    public INodeManager nodeManager { get; set; }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    public float sprintWeight => this.playerMovement.stanceRateMovement;
    public float changeStanceWeight = 4;
    public enum SprintManuver
    {
        Out,
        Stay
    }
    public SprintManuver sprintPhase;
    public PlayerSprintNode(Player player,PlayerStateNodeManager playerStateNodeManager, Func<bool> preCondition) : base(player, preCondition)
    {
        this.transitionAbleNode = new Dictionary<INode, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
        this.nodeManager = playerStateNodeManager;
    }

    private float sprintMaxSpeed => player.sprintMaxSpeed;
    private float sprintAcceletion => player.sprintAccelerate;
    private float sprintRotateSpeed => player.sprintRotateSpeed;

    private float sprintSpeedZone => player.StandMoveMaxSpeed + (Mathf.Abs(player.StandMoveMaxSpeed - player.sprintMaxSpeed)) * 0.7f;



    public override void Enter()
    {
        this.nodeLeafTransitionBehavior.TransitionAbleAll(this);
        if (player._movementCompoent.curMoveVelocity_World.magnitude <= sprintSpeedZone)
        {
            sprintDir = player.inputMoveDir_World;
        }
        else //(player.enemyMovement.curMoveVelocity_World.magnitude > sprintSpeedZone)
        {
            sprintDir = player._movementCompoent.forwardDir;
        }
        player.playerStance = Stance.stand;
        base.Enter();
    }

    public override bool IsReset()
    {
        if(this.TransitioningCheck())
            return false;

        return base.IsReset();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        this.playerMovement.SetStanceWeight(this.playerMovement.stanceRateMovement + Time.fixedDeltaTime * this.changeStanceWeight);
       
        SprintMaintainMomentum(sprintRotateSpeed, sprintRotateSpeed * 3f);
        base.FixedUpdateNode();
    }
    public override void Exit()
    {
        base.Exit();
    }
    private void SprintMaintainMomentum(float sprintDirRotateSpeed,float rotateCharSpeed)
    {
        this.sprintDir = Vector3.RotateTowards(this.sprintDir, this.player.inputMoveDir_World, sprintDirRotateSpeed * Time.deltaTime, 0);

        Vector3 targetMove = Vector3.Lerp(this.playerMovement.curMoveVelocity_World,this.sprintDir * this.sprintMaxSpeed * this.sprintWeight, Time.fixedDeltaTime * this.sprintAcceletion);

        this.playerMovement.UpdateMoveToDirWorld(targetMove, this.sprintAcceletion * this.sprintWeight, MoveMode.MaintainMomentumDirection);
        this.playerMovement.SetRotateToDirWorld(this.sprintDir.normalized, rotateCharSpeed);

    }

    public bool TransitioningCheck() => this.nodeLeafTransitionBehavior.TransitioningCheck(this);

    public void AddTransitionNode(INode node) => this.nodeLeafTransitionBehavior.AddTransistionNode(this, node);
    
}
