using System;
using UnityEngine;

public class PlayerCrouch_Move_NodeLeaf : PlayerStateNodeLeaf
{
    public PlayerCrouch_Move_NodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        MovementCompoent playerMovement = base.player._movementCompoent;

        playerMovement.UpdateMoveToDirWorld(this.player.inputMoveDir_World * this.player.CrouchMoveMaxSpeed * this.player.inputMoveDir_World.magnitude
            , this.player.CrouchMoveAccelerate * GetInverseDirectionalAccelMovement.GetInverseDirectionalAccel(this.player.inputMoveDir_World.normalized,this.player._movementCompoent.curMoveVelocity_World.normalized,this.player.changeDirAccel)
            , MoveMode.MaintainMomentumDirection);
        playerMovement.SetRotateToDirWorld(Camera.main.transform.forward, this.player.rotateSpeed);
        playerMovement.UpdateMovement();

        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
