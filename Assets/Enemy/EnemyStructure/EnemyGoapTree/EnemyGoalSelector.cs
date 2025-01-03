using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyGoalSelector : EnemyGoal
{
    public EnemyGoalSelector(EnemyControllerAPI enemyController, IEnemyGOAP enemyGOAP, Func<bool> preCondition) : base(enemyController, enemyGOAP)
    {
        this.preCondidtion = preCondition;

    }

    public override List<EnemyGoal> childNode { get; set; }
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
