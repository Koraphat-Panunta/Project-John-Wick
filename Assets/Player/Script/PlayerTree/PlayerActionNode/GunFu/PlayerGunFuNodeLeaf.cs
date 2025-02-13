using System;
using UnityEngine;

public class PlayerGunFuNodeLeaf : PlayerStateNodeLeaf
{
    IGunFuNode curGunFuNode;
    public PlayerGunFuNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
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
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return base.IsComplete();
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
