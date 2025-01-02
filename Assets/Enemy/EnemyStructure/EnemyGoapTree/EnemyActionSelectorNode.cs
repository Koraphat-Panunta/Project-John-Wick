using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyActionSelectorNode : EnemyActionNode
{
    public EnemyActionSelectorNode(EnemyControllerAPI enemyController, Func<bool> preCondition, Func<float> getCost) : base(enemyController)
    {
        this.preCondidtion = preCondition;
        this.getCost = getCost;
    }

    public override List<EnemyActionNode> childNode { get; set; }
    protected override Func<bool> preCondidtion { get; set; }
    protected override Func<float> getCost { get ; set ; }

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
