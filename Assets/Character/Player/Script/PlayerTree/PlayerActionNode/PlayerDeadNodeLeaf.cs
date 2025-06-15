using System;
using UnityEngine;

public class PlayerDeadNodeLeaf : PlayerStateNodeLeaf
{
    public PlayerDeadNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }

    public override void Enter()
    {
        player.NotifyObserver(player, SubjectPlayer.NotifyEvent.Dead);
        (player.playerMovement as IMovementCompoent).CancleMomentum();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        player.playerMovement.MoveToDirWorld(Vector3.zero, 10, 10, IMovementCompoent.MoveMode.IgnoreMomenTum);
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return false;
    }

    public override bool IsReset()
    {
        return false;
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
