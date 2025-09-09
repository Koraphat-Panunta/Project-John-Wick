using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemySprintStateNodeLeaf : EnemyStateLeafNode
{
    MovementCompoent enemyMovement => enemy._movementCompoent;
    
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

        enemyMovement.MoveToDirWorld(enemyMovement.forwardDir, enemy.sprintAccelerate, enemy.sprintMaxSpeed, MoveMode.IgnoreMomenTum);
        enemyMovement.RotateToDirWorld(enemy.lookRotationCommand, enemy.sprintRotateSpeed);

        base.FixedUpdateNode();
    }

  

    public override void UpdateNode()
    {
        
        base.UpdateNode();
    }
}
