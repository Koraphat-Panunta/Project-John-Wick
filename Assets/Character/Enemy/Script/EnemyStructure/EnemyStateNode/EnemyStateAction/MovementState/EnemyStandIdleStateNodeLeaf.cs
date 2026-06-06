using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandIdleStateNodeLeaf : EnemyStateLeafNode
{
    RotateObjectToward objectToward;
    public float decelerate = 4;
    MovementCompoent enemyMovement => enemy._movementCompoent;
    public EnemyStandIdleStateNodeLeaf(Enemy enemy,Func<bool> preCondition) : base(enemy,preCondition)
    {
        objectToward = new RotateObjectToward();
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
        enemyMovement.UpdateMoveToDirWorld(Vector3.zero, this.enemy.breakDecelerate, MoveMode.IgnoreMomentumDirection);
        enemyMovement.SetRotateToDirWorld(this.enemy.lookRotationCommand, this.enemy.rotateSpeed);
        enemyMovement.UpdateMovement();

        base.FixedUpdateNode();
    }

   

  

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
