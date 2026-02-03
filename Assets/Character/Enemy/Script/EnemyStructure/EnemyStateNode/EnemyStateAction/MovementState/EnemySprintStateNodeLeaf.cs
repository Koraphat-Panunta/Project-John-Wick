using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemySprintStateNodeLeaf : EnemyStateLeafNode
{
    MovementCompoent enemyMovement => enemy._movementCompoent;
    private Vector3 moveInputVelocity_WorldCommand;
    private Vector3 lookRotationCommand;


    public EnemySprintStateNodeLeaf(Enemy enemy,Func<bool> preCondition) : base(enemy,preCondition)
    {
   
    }

    public override void Enter()
    {
        enemy.enemyStance = Stance.stand;
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {

        enemyMovement.UpdateMoveToDirWorld(this.moveInputVelocity_WorldCommand * enemy.sprintMaxSpeed, enemy.sprintAccelerate, MoveMode.IgnoreMomentumDirection);
        enemyMovement.SetRotateToDirWorld(this.moveInputVelocity_WorldCommand.normalized, enemy.sprintRotateSpeed);

        base.FixedUpdateNode();
    }

  

    public override void UpdateNode()
    {
        this.moveInputVelocity_WorldCommand = enemy.moveInputVelocity_WorldCommand;
        this.lookRotationCommand = enemy.lookRotationCommand;

        base.UpdateNode();
    }
}
