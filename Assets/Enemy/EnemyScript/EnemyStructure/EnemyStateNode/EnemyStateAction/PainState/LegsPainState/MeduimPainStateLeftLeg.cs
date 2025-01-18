using System;
using System.Collections.Generic;
using UnityEngine;

public class MeduimPainStateLeftLeg : EnemyPainStateNodeLeaf
{
    public MeduimPainStateLeftLeg(Enemy enemy,Animator animator) : base(enemy, animator)
    {

    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    public override float painDuration { get; set; }
    public override IPainState.PainPart painPart { get; set; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    protected override string stateName => throw new NotImplementedException();

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
}
