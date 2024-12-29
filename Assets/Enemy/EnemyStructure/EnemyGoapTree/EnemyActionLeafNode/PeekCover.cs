using System;
using System.Collections.Generic;
using UnityEngine;

public class PeekCover : EnemyActionLeafNode
{
    public PeekCover(Enemy enemy) : base(enemy)
    {
    }

    public PeekCover(Enemy enemy, Func<bool> preCondition, Func<bool> isReset) : base(enemy, preCondition, isReset)
    {
    }

    public override List<EnemyActionNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override bool PreCondition()
    {
        return base.PreCondition();
    }

    public override void Update()
    {
        base.Update();
    }
}
