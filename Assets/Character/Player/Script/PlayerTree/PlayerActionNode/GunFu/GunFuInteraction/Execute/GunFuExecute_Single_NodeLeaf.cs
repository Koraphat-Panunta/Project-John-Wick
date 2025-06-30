using System;
using UnityEngine;

public class GunFuExecute_Single_NodeLeaf : PlayerStateNodeLeaf, IGunFuExecuteNodeLeaf
{

    public GunFuExecute_Single_NodeLeaf(Player player, Func<bool> preCondition,GunFuExecute_Single_NodeLeaf gunFuExecute_Single_NodeLeaf) : base(player, preCondition)
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
        if(this.IsComplete())
            return true;
        return false;
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
