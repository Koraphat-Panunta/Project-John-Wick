using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandTakeAimStateNodeLeaf : EnemyStateLeafNode
{
    ICoverUseable coverUseable;

    MovementCompoent movementCompoent => enemy._movementCompoent;
    public EnemyStandTakeAimStateNodeLeaf(Enemy enemy, Func<bool> preCondition, ICoverUseable coverUseable) : base(enemy,preCondition)
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
        switch (coverUseable.coverPoint)
        {
            case CoverPointTallSingleSide coverPointTallSingle:
                {
                    Vector3 moveDir = (coverUseable.peekPos - enemy.transform.position).normalized ;
                    if (Vector3.Distance(enemy.transform.position, coverUseable.peekPos) > 0.05f)
                    {
                        movementCompoent.UpdateMoveToDirWorld(moveDir * this.enemy.StandMoveMaxSpeed, this.enemy.StandMoveAccelerate, MoveMode.MaintainMomentumDirection);
                    }
                    else
                    {
                        movementCompoent.UpdateMoveToDirWorld(Vector3.zero, this.enemy.breakDecelerate, MoveMode.MaintainMomentumDirection);
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
                            movementCompoent.UpdateMoveToDirWorld(moveDir * this.enemy.StandMoveMaxSpeed, this.enemy.StandMoveAccelerate, MoveMode.MaintainMomentumDirection);
                        }
                        else
                        {
                            movementCompoent.UpdateMoveToDirWorld(Vector3.zero, this.enemy.breakDecelerate, MoveMode.MaintainMomentumDirection);
                        }
                    }
                    else
                    {
                        coverPointTallDouble.TakeThisCover(coverUseable, coverPointTallDouble.peekPosR);
                        Vector3 moveDir = (coverUseable.peekPos - enemy.transform.position).normalized ;
                        if (Vector3.Distance(enemy.transform.position, coverUseable.peekPos) > 0.05f)
                        {
                            this.movementCompoent.UpdateMoveToDirWorld(moveDir * this.enemy.StandMoveMaxSpeed, this.enemy.StandMoveAccelerate, MoveMode.MaintainMomentumDirection);
                        }
                        else
                        {
                            this.movementCompoent.UpdateMoveToDirWorld(Vector3.zero, this.enemy.breakDecelerate, MoveMode.MaintainMomentumDirection);
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
                        this.movementCompoent.UpdateMoveToDirWorld(moveDir * this.enemy.StandMoveMaxSpeed, this.enemy.StandMoveAccelerate, MoveMode.MaintainMomentumDirection);
                    }
                    else
                    {
                        movementCompoent.UpdateMoveToDirWorld(Vector3.zero, this.enemy.breakDecelerate, MoveMode.MaintainMomentumDirection);
                    }
                }
                break;
        }


        this.movementCompoent.SetRotateToDirWorld(this.enemy.lookRotationCommand, this.enemy.rotateSpeed);

        base.FixedUpdateNode();
    }

   

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
