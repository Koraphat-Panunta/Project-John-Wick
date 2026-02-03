using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandTakeCoverStateNodeLeaf : EnemyStateLeafNode
{
    ICoverUseable coverUseable;

    MovementCompoent movementCompoent => enemy._movementCompoent;
    public EnemyStandTakeCoverStateNodeLeaf(Enemy enemy,Func<bool> preCondition, ICoverUseable coverUseable) : base(enemy, preCondition)
    {
        this.coverUseable = coverUseable;

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
            movementCompoent.UpdateMoveToDirWorld(moveDir, this.enemy.StandMoveAccelerate, MoveMode.MaintainMomentumDirection);
        else
            movementCompoent.UpdateMoveToDirWorld(Vector3.zero, this.enemy.breakDecelerate, MoveMode.MaintainMomentumDirection);
              
        movementCompoent.SetRotateToDirWorld((this.enemy.targetKnewPos - this.enemy.transform.position).normalized, 6);

        base.UpdateNode();
    }
}
