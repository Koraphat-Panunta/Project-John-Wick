using System;
using UnityEngine;

public class PlayerDeadNodeLeaf : PlayerStateNodeLeaf
{
    public PlayerDeadNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }

    public override void Enter()
    {
        player.NotifyObserver(player,this);
        (player._movementCompoent as MovementCompoent).CancleMomentum();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        player._movementCompoent.MoveToDirWorld(Vector3.zero, 10, 10, MovementCompoent.MoveMode.IgnoreMomenTum);
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
