using System;
using System.Collections.Generic;
using UnityEngine;

public class FallDown : EnemyStateLeafNode
{
    public FallDown(Enemy enemy) : base(enemy)
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
        if (enemy.isDead)
            return true;

        return false;
    }

    public override bool PreCondition()
    {
        

        if (enemy.posture < 80|| enemy.GetHP()<=0)
            return true;

        return false;
    }

    public override void Update()
    {
        base.Update();
    }
}
