using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadStateNode : EnemyStateLeafNode
{
    public EnemyDeadStateNode(Enemy enemy) : base(enemy)
    {
    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        MotionControlManager motionControlManager = enemy.motionControlManager;
        motionControlManager.ChangeMotionState(motionControlManager.ragdollMotionState);

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.Dead);
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
        return enemy.isDead;
    }

   
}
