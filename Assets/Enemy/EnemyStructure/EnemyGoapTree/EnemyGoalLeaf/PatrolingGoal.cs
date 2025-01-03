using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolingGoal : EnemyGoalLeaf
{
    public PatrolingGoal(EnemyControllerAPI enemyController, IEnemyGOAP enemyGOAP) : base(enemyController, enemyGOAP)
    {

    }
    public override List<EnemyGoal> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }
    protected override EnemyActionLeafNode enemyActionLeaf { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    protected override EnemyActionSelectorNode startActionSelector { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

   

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override bool PreCondition()
    {
        return base.PreCondition();
    }

    public override void Update()
    {
        base.Update();
    }

    protected override void InitailizedActionNode()
    {
    }
 
}
