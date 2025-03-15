using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandTakeAimStateNode : EnemyStateLeafNode
{
    ICoverUseable coverUseable;
    NavMeshAgent agent;
    IMovementCompoent movementCompoent;
    public EnemyStandTakeAimStateNode(Enemy enemy, Func<bool> preCondition, ICoverUseable coverUseable) : base(enemy,preCondition)
    {
        this.coverUseable = coverUseable;
        agent = enemy.agent;
        this.movementCompoent = enemy.enemyMovement;
    }

    public override void Enter()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.TakeAim);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        switch (coverUseable.coverPoint)
        {
            case CoverPointTallSingleSide coverPointTallSingle:
                {
                    Vector3 moveDir = (coverUseable.peekPos - enemy.transform.position).normalized ;
                    if (Vector3.Distance(enemy.transform.position, coverUseable.coverPos) > 0.05f)
                    {
                        movementCompoent.MoveToDirWorld(moveDir, enemy.moveAccelerate, enemy.moveMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
                    }
                    else
                    {
                        movementCompoent.MoveToDirWorld(Vector3.zero, enemy.breakAccelerate, enemy.breakMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
                    }
                }
                break;

            case CoverPointTallDoubleSide coverPointTallDouble:
                {
                    if (coverUseable.coverPoint.CheckingTargetInCoverView(coverUseable, enemy.targetSpoterMask, coverPointTallDouble.peekPosL, out GameObject target))
                    {
                        coverPointTallDouble.TakeThisCover(coverUseable, coverPointTallDouble.peekPosL);
                        Vector3 moveDir = (coverUseable.peekPos - enemy.transform.position).normalized;
                        if (Vector3.Distance(enemy.transform.position, coverUseable.coverPos) > 0.05f)
                        {
                            movementCompoent.MoveToDirWorld(moveDir, enemy.moveAccelerate, enemy.moveMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
                        }
                        else
                        {
                            movementCompoent.MoveToDirWorld(Vector3.zero, enemy.breakAccelerate, enemy.breakMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
                        }
                    }
                    else
                    {
                        coverPointTallDouble.TakeThisCover(coverUseable, coverPointTallDouble.peekPosR);
                        Vector3 moveDir = (coverUseable.peekPos - enemy.transform.position).normalized ;
                        if (Vector3.Distance(enemy.transform.position, coverUseable.coverPos) > 0.05f)
                        {
                            movementCompoent.MoveToDirWorld(moveDir, enemy.moveAccelerate, enemy.moveMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
                        }
                        else
                        {
                            movementCompoent.MoveToDirWorld(Vector3.zero, enemy.breakAccelerate, enemy.breakMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
                        }
                    }
                }
                break;

            case CoverPointShort coverPointShort:
                {
                    coverPointShort.TakeThisCover(coverUseable);
                    Vector3 moveDir = (coverUseable.peekPos - enemy.transform.position).normalized;
                    if (Vector3.Distance(enemy.transform.position, coverUseable.coverPos) > 0.05f)
                    {
                        movementCompoent.MoveToDirWorld(moveDir, enemy.moveAccelerate, enemy.moveMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
                    }
                    else
                    {
                        movementCompoent.MoveToDirWorld(Vector3.zero, enemy.breakAccelerate, enemy.breakMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
                    }
                }
                break;
        }


        movementCompoent.RotateToDirWorld(enemy.lookRotationCommand, enemy.aimingRotateSpeed);

        base.FixedUpdateNode();
    }

   

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
