using System;
using UnityEngine;

public class BlockStateNodeLeaf : EnemyStateLeafNode
{
    // TODO: replace _blockDuration with AnimationTriggerEventPlayer once animation is wired
    private float _blockDuration;
    private float _timer;

    public BlockStateNodeLeaf(Enemy enemy, Func<bool> preCondition, float blockDuration)
        : base(enemy, preCondition)
    {
        _blockDuration = blockDuration;
    }

    public override void Enter()
    {
        Debug.Log("BlockEnter");
        _timer = 0;
        base.Enter();
    }

    public override void UpdateNode()
    {
        _timer += Time.deltaTime;
        if (_timer >= _blockDuration)
            isComplete = true;

        this.enemy._movementCompoent.UpdateMoveToDirWorld(Vector3.zero, this.enemy.breakDecelerate, MoveMode.IgnoreMomentumDirection);
        

        base.UpdateNode();
    }

    public override bool IsReset()
    {
        if (enemy.isDead)
            return true;

        return IsComplete();
    }
}
