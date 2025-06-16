using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandIdleStateNodeLeaf : EnemyStateLeafNode
{
    RotateObjectToward objectToward;
    NavMeshAgent agent;
    public float decelerate = 4;
    IMovementCompoent enemyMovement;
    public EnemyStandIdleStateNodeLeaf(Enemy enemy,Func<bool> preCondition) : base(enemy,preCondition)
    {
        objectToward = new RotateObjectToward();
        agent = enemy.agent;
        enemyMovement = enemy.enemyMovement;
       

    }

    public override void Enter()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        enemy.NotifyObserver(enemy, this);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        enemyMovement.MoveToDirWorld(Vector3.zero, enemy.breakAccelerate, enemy.breakMaxSpeed,IMovementCompoent.MoveMode.MaintainMomentum);
        enemyMovement.RotateToDirWorld(enemy.lookRotationCommand, enemy.moveRotateSpeed);

        base.FixedUpdateNode();
    }

   

  

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
