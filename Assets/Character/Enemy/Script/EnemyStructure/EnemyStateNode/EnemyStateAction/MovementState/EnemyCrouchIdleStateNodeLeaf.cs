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
        movementCompoent.UpdateMoveToDirWorld(Vector3.zero, this.enemy.breakDecelerate, MoveMode.IgnoreMomentumDirection);
        this.movementCompoent.SetRotateToDirWorld(lookRotationCommand, this.enemy.rotateSpeed);

        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        this.lookRotationCommand = enemy.lookRotationCommand;
        base.UpdateNode();
    }
}
