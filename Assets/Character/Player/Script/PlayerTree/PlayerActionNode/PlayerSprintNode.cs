using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintNode : PlayerStateNodeLeaf
{
    private PlayerMovement playerMovement => player.playerMovement;
    private Vector3 sprintDir;
    public enum SprintPhase
    {
        Out,
        Stay
    }
    public SprintPhase sprintPhase;

    public PlayerSprintNode(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
        
    }

    private float sprintMaxSpeed => player.sprintMaxSpeed;
    private float sprintAcceletion => player.sprintAccelerate;
    private float sprintRotateSpeed => player.sprintRotateSpeed;

    private float sprintSpeedZone => player.StandMoveMaxSpeed + (Mathf.Abs(player.StandMoveMaxSpeed - player.sprintMaxSpeed)) * 0.7f;


    public override void Enter()
    {
        if (player.playerMovement.curMoveVelocity_World.magnitude <= sprintSpeedZone)
        {
            sprintDir = player.inputMoveDir_World;
            sprintPhase = SprintPhase.Out;
        }
        else //(player.playerMovement.curMoveVelocity_World.magnitude > sprintSpeedZone)
        {
            sprintDir = player.playerMovement.forwardDir;
            sprintPhase = SprintPhase.Stay;
        }
        player.playerStance = Player.PlayerStance.stand;
        player.NotifyObserver(player,this);
        
        base.Enter();
    }
    public override void UpdateNode()
    {
        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        if (sprintPhase == SprintPhase.Out)
        {
            SprintMaintainMomentum();
            if (Vector3.Dot(player.playerMovement.forwardDir.normalized, sprintDir.normalized) >= 0.95f
                && playerMovement.curMoveVelocity_World.magnitude >= sprintSpeedZone * 0.95f)
                sprintPhase = SprintPhase.Stay;
        }
        else if (sprintPhase == SprintPhase.Stay)
        {
            playerMovement.MoveToDirWorld(playerMovement.forwardDir, sprintAcceletion, sprintMaxSpeed, IMovementCompoent.MoveMode.IgnoreMomenTum);
            playerMovement.RotateToDirWorld(player.inputMoveDir_World, sprintRotateSpeed);
        }
        
        base.FixedUpdateNode();
    }
    private void SprintMaintainMomentum()
    {
        float sprintDirRotateSpeed = sprintRotateSpeed * 4f;
        float rotateCharSpeed = sprintRotateSpeed * 1.24f;

        sprintDir = Vector3.RotateTowards(sprintDir, player.inputMoveDir_World, sprintDirRotateSpeed * Time.deltaTime, 0);
        playerMovement.MoveToDirWorld(sprintDir, sprintAcceletion, sprintSpeedZone, IMovementCompoent.MoveMode.MaintainMomentum);
        playerMovement.RotateToDirWorld(sprintDir, rotateCharSpeed);

    }
   
   
}
