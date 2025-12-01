using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandTakeAimStateNodeLeaf : EnemyStateLeafNode
{
    ICoverUseable coverUseable;
    NavMeshAgent agent;
    MovementCompoent movementCompoent => enemy._movementCompoent;
    public EnemyStandTakeAimStateNodeLeaf(Enemy enemy, Func<bool> preCondition, ICoverUseable coverUseable) : base(enemy,preCondition)
    {
        this.coverUseable = coverUseable;
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
        switch (coverUseable.coverPoint)
        {
            case CoverPointTallSingleSide coverPointTallSingle:
                {
                    Vector3 moveDir = (coverUseable.peekPos - enemy.transform.position).normalized ;
                    if (Vector3.Distance(enemy.transform.position, coverUseable.peekPos) > 0.05f)
                    {
                        movementCompoent.UpdateMoveToDirWorld(moveDir, enemy.moveAccelerate, enemy.moveMaxSpeed, MoveMode.MaintainMomentum);
                    }
                    else
                    {
                        movementCompoent.UpdateMoveToDirWorld(Vector3.zero, enemy.breakAccelerate, enemy.breakMaxSpeed, MoveMode.MaintainMomentum);
                    }
                }
                break;

            case CoverPointTallDoubleSide coverPointTallDouble:
                {
                    if (coverUseable.coverPoint.CheckingTargetInCoverView(coverUseable, enemy.targetSpoterMask, coverPointTallDouble.peekPosL, out GameObject target))
                    {
                        coverPointTallDouble.TakeThisCover(coverUseable, coverPointTallDouble.peekPosL);
                        Vector3 moveDir = (coverUseable.peekPos - enemy.transform.position).normalized;
                        if (Vector3.Distance(enemy.transform.position, coverUseable.peekPos) > 0.05f)
                        {
                            movementCompoent.UpdateMoveToDirWorld(moveDir, enemy.moveAccelerate, enemy.moveMaxSpeed, MoveMode.MaintainMomentum);
                        }
                        else
                        {
                            movementCompoent.UpdateMoveToDirWorld(Vector3.zero, enemy.breakAccelerate, enemy.breakMaxSpeed, MoveMode.MaintainMomentum);
                        }
                    }
                    else
                    {
                        coverPointTallDouble.TakeThisCover(coverUseable, coverPointTallDouble.peekPosR);
                        Vector3 moveDir = (coverUseable.peekPos - enemy.transform.position).normalized ;
                        if (Vector3.Distance(enemy.transform.position, coverUseable.peekPos) > 0.05f)
                        {
                            movementCompoent.UpdateMoveToDirWorld(moveDir, enemy.moveAccelerate, enemy.moveMaxSpeed, MoveMode.MaintainMomentum);
                        }
                        else
                        {
                            movementCompoent.UpdateMoveToDirWorld(Vector3.zero, enemy.breakAccelerate, enemy.breakMaxSpeed, MoveMode.MaintainMomentum);
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
                        movementCompoent.UpdateMoveToDirWorld(moveDir, enemy.moveAccelerate, enemy.moveMaxSpeed, MoveMode.MaintainMomentum);
                    }
                    else
                    {
                        movementCompoent.UpdateMoveToDirWorld(Vector3.zero, enemy.breakAccelerate, enemy.breakMaxSpeed, MoveMode.MaintainMomentum);
                    }
                }
                break;
        }


        movementCompoent.SetRotateToDirWorld(enemy.lookRotationCommand, enemy.aimingRotateSpeed);

        base.FixedUpdateNode();
    }

   

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
