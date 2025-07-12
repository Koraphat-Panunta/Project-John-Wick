using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadStateNode : EnemyStateLeafNode
{
    MovementCompoent movementCompoent => enemy._movementCompoent;
    public EnemyDeadStateNode(Enemy enemy,Func<bool> preCondition) : base(enemy, preCondition)
    {
    }

    public override void Enter()
    {
        MotionControlManager motionControlManager = enemy.motionControlManager;
        motionControlManager.ChangeMotionState(motionControlManager.ragdollMotionState);

        enemy.gameObject.layer = 2; //IgnoreRayCast
        enemy.NotifyObserver(enemy,this);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override bool IsReset()
    {
        return false;
    }
    public override void FixedUpdateNode()
    {
        this.movementCompoent.CancleMomentum();
        base.FixedUpdateNode();
    }
}
