using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintNode : PlayerStateNodeLeaf
{
    private PlayerMovement playerMovement => player._movementCompoent as PlayerMovement;
    private Vector3 sprintDir;

    public float sprintWeight => this.playerMovement.stanceRateMovement;
    public float changeStanceWeight = 4;
    public enum SprintManuver
    {
        Out,
        Stay
    }
    public SprintManuver sprintPhase;
    public PlayerSprintNode(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
        
    }

    private float sprintMaxSpeed => player.sprintMaxSpeed;
    private float sprintAcceletion => player.sprintAccelerate;
    private float sprintRotateSpeed => player.sprintRotateSpeed;

    private float sprintSpeedZone => player.StandMoveMaxSpeed + (Mathf.Abs(player.StandMoveMaxSpeed - player.sprintMaxSpeed)) * 0.7f;


    public override void Enter()
    {
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
   
   
}
