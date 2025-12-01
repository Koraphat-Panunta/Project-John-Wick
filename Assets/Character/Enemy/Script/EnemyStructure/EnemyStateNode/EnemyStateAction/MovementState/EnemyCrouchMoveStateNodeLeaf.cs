using System;
using UnityEngine;

public class EnemyCrouchMoveStateNodeLeaf : EnemyStateLeafNode
{
    private MovementCompoent movementCompoent => enemy._movementCompoent;
    private Vector3 moveInputVelocity_WorldCommand;
    private Vector3 lookRotationCommand;
    public EnemyCrouchMoveStateNodeLeaf(Enemy enemy, Func<bool> preCondition) : base(enemy, preCondition)
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
        this.movementCompoent.UpdateMoveToDirWorld(this.moveInputVelocity_WorldCommand, enemy.CrouchMoveAccelerate, enemy.CrouchMoveMaxSpeed , MoveMode.MaintainMomentum);
        this.movementCompoent.SetRotateToDirWorld(lookRotationCommand, enemy.moveRotateSpeed);
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        this.moveInputVelocity_WorldCommand = enemy.moveInputVelocity_WorldCommand;
        this.lookRotationCommand = enemy.lookRotationCommand;
        base.UpdateNode();
    }
}
