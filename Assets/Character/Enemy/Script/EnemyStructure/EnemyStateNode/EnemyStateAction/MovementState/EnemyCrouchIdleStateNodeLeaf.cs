using System;
using UnityEngine;

public class EnemyCrouchIdleStateNodeLeaf : EnemyStateLeafNode
{
    private MovementCompoent movementCompoent => enemy._movementCompoent;
    public EnemyCrouchIdleStateNodeLeaf(Enemy enemy, Func<bool> preCondition) : base(enemy, preCondition)
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

        movementCompoent.MoveToDirWorld(Vector3.zero, enemy.breakAccelerate, enemy.breakMaxSpeed, MoveMode.MaintainMomentum);

        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
