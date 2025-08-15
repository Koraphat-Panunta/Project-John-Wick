using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintNode : PlayerStateNodeLeaf
{
    private PlayerMovement playerMovement => player._movementCompoent as PlayerMovement;
    private Vector3 sprintDir;
    private float sprintStance;
    private float sprintStanceChangeRate = 4;
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
            sprintPhase = SprintManuver.Out;
        }
        else //(player.playerMovement.curMoveVelocity_World.magnitude > sprintSpeedZone)
        {
            sprintDir = player._movementCompoent.forwardDir;
            sprintPhase = SprintManuver.Stay;
        }
        player.playerStance = Player.PlayerStance.stand;
        sprintStance = 0;
        base.Enter();
    }
    public override void UpdateNode()
    {
        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        if (sprintPhase == SprintManuver.Out)
        {
            SprintMaintainMomentum(sprintRotateSpeed * 2.75f, sprintRotateSpeed * 1.65f);
            if (Vector3.Dot(player._movementCompoent.forwardDir.normalized, sprintDir.normalized) >= 0.95f
                && playerMovement.curMoveVelocity_World.magnitude >= sprintSpeedZone * 0.95f)
                sprintPhase = SprintManuver.Stay;
        }
        else if (sprintPhase == SprintManuver.Stay)
        {
            //playerMovement.MoveToDirWorld(playerMovement.forwardDir, sprintAcceletion, sprintMaxSpeed, MoveMode.IgnoreMomenTum);
            //playerMovement.RotateToDirWorld(player.inputMoveDir_World, sprintRotateSpeed);
            SprintMaintainMomentum(sprintRotateSpeed * 2.75f, sprintRotateSpeed * 1.65f);
        }
        sprintStance = Mathf.Clamp01(sprintStance + Time.deltaTime * sprintStanceChangeRate);
        base.FixedUpdateNode();
    }
    public override void Exit()
    {
        base.Exit();
    }
    private void SprintMaintainMomentum(float sprintDirRotateSpeed,float rotateCharSpeed)
    {
        sprintDir = Vector3.RotateTowards(sprintDir, player.inputMoveDir_World, sprintDirRotateSpeed * Time.deltaTime, 0);
        playerMovement.MoveToDirWorld(sprintDir.normalized, sprintAcceletion * sprintStance, sprintSpeedZone, MoveMode.MaintainMomentum);
        playerMovement.RotateToDirWorld(sprintDir.normalized, rotateCharSpeed);

    }
   
   
}
