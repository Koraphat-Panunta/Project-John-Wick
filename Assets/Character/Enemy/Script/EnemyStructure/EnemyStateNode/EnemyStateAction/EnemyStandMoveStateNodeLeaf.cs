using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandMoveStateNodeLeaf : EnemyStateLeafNode
{
    
    RotateObjectToward objectToward;
    NavMeshAgent agent;
    IMovementCompoent enemyMovement;
  
    public EnemyStandMoveStateNodeLeaf(Enemy enemy, Func<bool> preCondition) : base(enemy, preCondition)
    {
        this.objectToward = new RotateObjectToward();
        this.agent = enemy.agent;
        this.enemyMovement = enemy.enemyMovement;
    }

   
    public override void Enter()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        enemy.NotifyObserver(enemy,this);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {

        this.enemyMovement.MoveToDirWorld(enemy.moveInputVelocity_WorldCommand, enemy.moveAccelerate, enemy.moveMaxSpeed, IMovementCompoent.MoveMode.IgnoreMomenTum);
        this.enemyMovement.RotateToDirWorld(enemy.lookRotationCommand, enemy.moveRotateSpeed);

        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
