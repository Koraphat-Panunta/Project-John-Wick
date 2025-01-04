using System;
using System.Collections.Generic;
using UnityEngine;

public class RagDoll : EnemyStateLeafNode
{
    public RagDoll(Enemy enemy) : base(enemy)
    {
    }
    
    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.ragdollMotionState);
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
        return false;
    }

    public override bool PreCondition()
    {
        if (enemy.pressure > 80)
            return true;

        return false;
    }

    public override void Update()
    {
        base.Update();
    }
}
