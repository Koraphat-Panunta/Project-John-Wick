using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandTakeCoverStateNodeLeaf : EnemyStateLeafNode
{
    ICoverUseable coverUseable;
    RotateObjectToward rotateObject;
    NavMeshAgent agent;
    MovementCompoent movementCompoent => enemy._movementCompoent;
    public EnemyStandTakeCoverStateNodeLeaf(Enemy enemy,Func<bool> preCondition, ICoverUseable coverUseable) : base(enemy, preCondition)
    {
        this.coverUseable = coverUseable;
        rotateObject = new RotateObjectToward();
        agent = enemy.agent;
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
        base.FixedUpdateNode();
    }

   

   

    public override void UpdateNode()
    {
        
        Vector3 CoverPos = coverUseable.coverPos;

        Vector3 moveDir = (CoverPos - enemy.transform.position).normalized;

        if (Vector3.Distance(enemy.transform.position, CoverPos) > 0.15f)
            movementCompoent.MoveToDirWorld(moveDir, enemy.moveAccelerate, enemy.moveMaxSpeed, MoveMode.MaintainMomentum);
        else
            movementCompoent.MoveToDirWorld(Vector3.zero, enemy.breakAccelerate, enemy.breakMaxSpeed, MoveMode.MaintainMomentum);
              

        movementCompoent.RotateToDirWorld(coverUseable.coverPoint.coverDir, 6);

        base.UpdateNode();
    }
}
