using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemySprintStateNode : EnemyStateLeafNode
{
    IMovementCompoent enemyMovement;
    
    public EnemySprintStateNode(Enemy enemy,Func<bool> preCondition) : base(enemy,preCondition)
    {
        enemyMovement = enemy.enemyMovement;        
    }

    public override void Enter()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.Sprint);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {

        enemyMovement.MoveToDirWorld(enemyMovement.forwardDir, enemy.sprintAccelerate, enemy.sprintMaxSpeed, IMovementCompoent.MoveMode.IgnoreMomenTum);
        enemyMovement.RotateToDirWorld(enemy.lookRotationCommand, enemy.sprintRotateSpeed);

        base.FixedUpdateNode();
    }

  

    public override void UpdateNode()
    {
        
        base.UpdateNode();
    }
}
