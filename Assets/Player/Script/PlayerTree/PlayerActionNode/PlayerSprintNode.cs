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

    private float sprintSpeedZone => player.StandMoveMaxSpeed + (Mathf.Abs(player.StandMoveMaxSpeed - player.sprintMaxSpeed)) * 0.5f;


    public override void Enter()
    {
        if (player.playerMovement.curMoveVelocity_World.magnitude <= sprintSpeedZone)
        {
            sprintDir = player.inputMoveDir_World;
            sprintPhase = SprintPhase.Out;
            outSprinttimer = 0;
        }
        else //(player.playerMovement.curMoveVelocity_World.magnitude > sprintSpeedZone)
        {
            sprintDir = player.playerMovement.forwardDir;
            sprintPhase = SprintPhase.Stay;
        }
        player.playerStance = Player.PlayerStance.stand;
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.Sprint);
        
        base.Enter();
    }
    public override void UpdateNode()
    {
        InputPerformed();

        base.UpdateNode();
    }
    private RotateObjectToward rotate = new RotateObjectToward();
    private float outSprinttimer;
    private float outSprintDuration = 1.5f;
    public override void FixedUpdateNode()
    {
        if (sprintPhase == SprintPhase.Out)
        {
            outSprinttimer += Time.deltaTime;

            float rotT = (1 / Mathf.Pow(outSprintDuration, 2.6f)) * (Mathf.Pow(outSprinttimer, 2.6f));

            sprintDir = Vector3.RotateTowards(sprintDir, player.inputMoveDir_World, sprintRotateSpeed *0.9f * Time.deltaTime, 0);
            playerMovement.MoveToDirWorld(sprintDir, sprintAcceletion, sprintSpeedZone, IMovementCompoent.MoveMode.MaintainMomentum);

            Debug.Log("rotate t =" + rotT);
            playerMovement.RotateToDirWorldSlerp(sprintDir, rotT);

            if (outSprinttimer >= outSprintDuration)
                sprintPhase = SprintPhase.Stay;
        }
        else if(sprintPhase == SprintPhase.Stay)
        {
            playerMovement.MoveToDirWorld(playerMovement.forwardDir, sprintAcceletion, sprintMaxSpeed, IMovementCompoent.MoveMode.IgnoreMomenTum);
            playerMovement.RotateToDirWorld(player.inputMoveDir_World, sprintRotateSpeed);
        }

        base.FixedUpdateNode();
    }

    private  void InputPerformed()
    {
        if (player.isSwapShoulder)
        {
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwapShoulder);
        }
    }
   
}
