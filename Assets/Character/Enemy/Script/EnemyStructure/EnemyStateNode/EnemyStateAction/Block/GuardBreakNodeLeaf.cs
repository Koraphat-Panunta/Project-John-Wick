using System;
using UnityEngine;

public class GuardBreakNodeLeaf : EnemyStateLeafNode
{
    private float _breakDuration;
    private float _timer;

    public GuardBreakNodeLeaf(
        Enemy enemy
        , Func<bool> preCondition
        , float breakDuration)
        : base(enemy, preCondition)
    {
        _breakDuration = breakDuration;
    }

    public override void Enter()
    {
        this._timer = 0;
        base.Enter();
    }

    public override void UpdateNode()
    {
        this._timer += Time.deltaTime;
        if (this._timer >= _breakDuration)
            this.isComplete = true;

        base.UpdateNode();
    }

    public override bool IsReset()
    {
        if (this.enemy.isDead)
            return true;

        if (this.enemy._isPainTrigger)
            return true;

        if (this.enemy._triggerEnterGotAttacked_OCM)
            return true;

        if (this.enemy.isDead)
            return true;

        return IsComplete();
    }
}
