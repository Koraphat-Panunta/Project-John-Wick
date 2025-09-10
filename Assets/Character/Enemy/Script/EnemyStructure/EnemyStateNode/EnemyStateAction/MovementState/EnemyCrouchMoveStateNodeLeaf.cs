using System;
using UnityEngine;

public class EnemyCrouchMoveStateNodeLeaf : EnemyStateLeafNode
{
    private MovementCompoent movementCompoent;
    public EnemyCrouchMoveStateNodeLeaf(Enemy enemy, Func<bool> preCondition) : base(enemy, preCondition)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {

        movementCompoent.MoveToDirWorld(enemy.moveInputVelocity_WorldCommand, enemy.CrouchMoveAccelerate, enemy.CrouchMoveMaxSpeed * enemy.moveInputVelocity_WorldCommand.magnitude, MoveMode.MaintainMomentum);

        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
