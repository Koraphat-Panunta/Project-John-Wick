using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandMoveNodeLeaf : PlayerStateNodeLeaf
{
    PlayerMovement playerMovement => this.player._movementCompoent as PlayerMovement;
    public float moveStanceWeight => 1 - this.playerMovement.movementAttribute.stanceRate;
    public float changeStanceWeightRate = 5;

    
    public PlayerStandMoveNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }

    public override void Enter()
    {
        this.player.NotifyObserver(player, this);
        base.Enter();
    }
    
    public override void FixedUpdateNode()
    {
        

        this.playerMovement.SetStanceWeight(this.playerMovement.movementAttribute.stanceRate - this.changeStanceWeightRate * Time.fixedDeltaTime);

        float inversAccel = GetInverseDirectionalAccelMovement.GetInverseDirectionalAccel(this.player.inputMoveDir_World.normalized, this.playerMovement.curMoveVelocity_World.normalized, this.player.changeDirAccel);

        
        this.playerMovement.SetRotateToDirWorld(this.player.cinemachineCamera.targetDir
            , SlowDownRotateSpeed.GetSlowDownRotateSpeedOnNearlyTargetRotation(this.playerMovement.forwardDir, this.player.cinemachineCamera.targetDir,30,this.player.rotateSpeed));

        this.playerMovement.UpdateMovement();
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
         float inversAccel = GetInverseDirectionalAccelMovement.GetInverseDirectionalAccel(this.player.inputMoveDir_World.normalized, this.playerMovement.curMoveVelocity_World.normalized, this.player.changeDirAccel);
        this.playerMovement.UpdateMoveToDirWorld(
            this.player.inputMoveDir_World * this.player.StandMoveMaxSpeed
            , this.player.StandMoveAccelerate * this.moveStanceWeight * inversAccel
            , MoveMode.MaintainMomentumDirection
            );

        base.UpdateNode();
    }
}
