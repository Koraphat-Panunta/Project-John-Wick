using System;
using UnityEngine;

public class EnemyCrouchIdleStateNodeLeaf : EnemyStateLeafNode
{
    private MovementCompoent movementCompoent => enemy._movementCompoent;
    private Vector3 lookRotationCommand;
    public EnemyCrouchIdleStateNodeLeaf(Enemy enemy, Func<bool> preCondition) : base(enemy, preCondition)
    {

    }
    public override void Enter()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        movementCompoent.MoveToDirWorld(Vector3.zero, enemy.breakAccelerate, enemy.breakMaxSpeed, MoveMode.MaintainMomentum);
        this.movementCompoent.RotateToDirWorld(lookRotationCommand, enemy.moveRotateSpeed);

        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        this.lookRotationCommand = enemy.lookRotationCommand;
        base.UpdateNode();
    }
}
