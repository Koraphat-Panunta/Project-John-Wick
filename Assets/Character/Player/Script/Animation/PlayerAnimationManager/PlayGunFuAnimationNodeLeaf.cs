using System;
using UnityEngine;

public class PlayGunFuAnimationNodeLeaf : AnimationNodeLeaf
{
    private IGunFuAble gunFuAble;
    private IGunFuNode gunFuNode;
    private string curStateName;
    public PlayGunFuAnimationNodeLeaf(Func<bool> preCondition,IGunFuAble gunFuAble,) : base(preCondition)
    {
        this.gunFuAble = gunFuAble;
    }
    public override bool Precondition()
    {
        if(gunFuNode == null)
            return false;

        return base.Precondition();
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

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
