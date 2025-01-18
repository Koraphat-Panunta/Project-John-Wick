using System;
using System.Collections.Generic;
using UnityEngine;

public class HeavyPainStateFrontBody : EnemyPainStateNodeLeaf
{
    public HeavyPainStateFrontBody(Enemy enemy, Animator animator) : base(enemy, animator)
    {
        painDuration = enemy._painDurScrp.bodyFront_HeavyHit;
        painPart = IPainState.PainPart.BodyFornt;
    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    public override float painDuration { get; set; }
    public override IPainState.PainPart painPart { get; set; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    protected override string stateName => "BodyFont_Heavy";

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
