using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandMoveStateNodeLeaf : EnemyStateLeafNode
{
    

    MovementCompoent enemyMovement => enemy._movementCompoent;
    private Vector3 moveInputVelocity_WorldCommand;
    private Vector3 lookRotationCommand;

    public EnemyStandMoveStateNodeLeaf(Enemy enemy, Func<bool> preCondition) : base(enemy, preCondition)
    {

    }

   
    public override void Enter()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        this.enemyMovement.UpdateMoveToDirWorld(this.moveInputVelocity_WorldCommand.normalized * this.enemy.StandMoveMaxSpeed, this.enemy.StandMoveAccelerate, MoveMode.IgnoreMomentumDirection);
        this.enemyMovement.SetRotateToDirWorld(this.lookRotationCommand, this.enemy.rotateSpeed);
        this.enemyMovement.UpdateMovement();

        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        this.moveInputVelocity_WorldCommand = enemy.moveInputVelocity_WorldCommand;
        this.lookRotationCommand = enemy.lookRotationCommand;
        base.UpdateNode();
    }
}
