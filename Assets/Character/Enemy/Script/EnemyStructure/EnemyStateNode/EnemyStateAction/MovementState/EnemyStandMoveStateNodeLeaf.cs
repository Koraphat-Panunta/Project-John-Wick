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
        this.enemyMovement.MoveToDirWorld(moveInputVelocity_WorldCommand, enemy.moveAccelerate, enemy.moveMaxSpeed, MoveMode.MaintainMomentum);
        this.enemyMovement.RotateToDirWorld(lookRotationCommand, enemy.moveRotateSpeed);

        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        this.moveInputVelocity_WorldCommand = enemy.moveInputVelocity_WorldCommand;
        this.lookRotationCommand = enemy.lookRotationCommand;
        base.UpdateNode();
    }
}
