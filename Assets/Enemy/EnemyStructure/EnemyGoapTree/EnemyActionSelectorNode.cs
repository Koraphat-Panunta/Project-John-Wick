using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyActionSelectorNode : EnemyActionNode
{
    public EnemyActionSelectorNode(EnemyCommandAPI enemyController, Func<bool> preCondition) : base(enemyController)
    {
        this.preCondidtion = preCondition;
    }

    public override List<EnemyActionNode> childNode { get; set; }
    protected override Func<bool> preCondidtion { get; set; }

    public override void FixedUpdate()
    {

    }

    public override bool IsReset()
    {
        return true;
    }

    public override bool PreCondition()
    {
        return preCondidtion.Invoke();
    }

    public override void Update()
    {

    }
}
