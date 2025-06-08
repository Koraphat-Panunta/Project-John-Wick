using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandTakeCoverStateNodeLeaf : EnemyStateLeafNode
{
    ICoverUseable coverUseable;
    RotateObjectToward rotateObject;
    NavMeshAgent agent;
    IMovementCompoent movementCompoent;
    public EnemyStandTakeCoverStateNodeLeaf(Enemy enemy,Func<bool> preCondition, ICoverUseable coverUseable) : base(enemy, preCondition)
    {
        this.coverUseable = coverUseable;
        rotateObject = new RotateObjectToward();
        agent = enemy.agent;
        movementCompoent = enemy.enemyMovement;
    }

    public override void Enter()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.TakeCover);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

   

   

    public override void UpdateNode()
    {
        
        Vector3 CoverPos = coverUseable.coverPos;

        Vector3 moveDir = (CoverPos - enemy.transform.position).normalized;

        if (Vector3.Distance(enemy.transform.position, CoverPos) > 0.15f)
            movementCompoent.MoveToDirWorld(moveDir, enemy.moveAccelerate, enemy.moveMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
        else
            movementCompoent.MoveToDirWorld(Vector3.zero, enemy.breakAccelerate, enemy.breakMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
              

        movementCompoent.RotateToDirWorld(coverUseable.coverPoint.coverDir, 6);

        base.UpdateNode();
    }
}
