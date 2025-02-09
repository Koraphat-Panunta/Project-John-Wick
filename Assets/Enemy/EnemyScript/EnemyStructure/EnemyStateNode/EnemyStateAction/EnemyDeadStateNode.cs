using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadStateNode : EnemyStateLeafNode
{
    IMovementCompoent movementCompoent;
    public EnemyDeadStateNode(Enemy enemy,Func<bool> preCondition) : base(enemy, preCondition)
    {
        this.movementCompoent = enemy.enemyMovement;
    }

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
