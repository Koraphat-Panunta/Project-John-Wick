using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyGoalSelector : EnemyGoal
{
    public EnemyGoalSelector(EnemyControllerAPI enemyController, IEnemyGOAP enemyGOAP, Func<bool> preCondition,Func<float> getCost) : base(enemyController, enemyGOAP)
    {
        this.preCondidtion = preCondition;
        this.getCost = getCost;
    }

    public override List<EnemyGoal> childNode { get; set; }
    protected override Func<bool> preCondidtion { get; set; }
    protected override Func<float> getCost { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public override void FixedUpdate()
    {

    }

    public override float GetCost()
    {
        throw new NotImplementedException();
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
