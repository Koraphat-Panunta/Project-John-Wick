using System;
using System.Collections.Generic;
using UnityEngine;

public class HeavyPainStateLeftLeg : EnemyPainStateNodeLeaf
{
    public HeavyPainStateLeftLeg(Enemy enemy, Animator animator) : base(enemy, animator)
    {
        painDuration = enemy._painDurScrp.legLeft_HeavyHit;
        painPart = IPainState.PainPart.LegLeft;
    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    public override float painDuration { get; set; }
    public override IPainState.PainPart painPart { get; set; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    protected override string stateName => "LeftLeg_Heavy";

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
        if (enemy.isDead)
            return true;

        if (time >= painDuration)
            return true;

        if (enemy._isPainTrigger)
            return true;

        return false;
    }

    public override bool PreCondition()
    {
        if (enemy._painPart == painPart
           && enemy.posture <= enemy._postureHeavy)
            return true;

        return false;
    }

    public override void Update()
    {
        base.Update();
    }
}
