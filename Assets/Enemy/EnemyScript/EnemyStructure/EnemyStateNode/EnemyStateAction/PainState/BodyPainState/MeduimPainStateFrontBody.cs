using System;
using System.Collections.Generic;
using UnityEngine;

public class MeduimPainStateFrontBody : EnemyPainStateNodeLeaf
{
    public MeduimPainStateFrontBody(Enemy enemy, Animator animator) : base(enemy, animator)
    {
        painDuration = enemy._painDurScrp.bodyFornt_MediumHit;
        painPart = IPainState.PainPart.BodyFornt;
    }
    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    public override float painDuration { get; set; }
    public override IPainState.PainPart painPart { get; set; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    protected override string stateName => "BodyFont_Mid";

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
           && enemy._posture <= enemy._postureMedium)
            return true;

        return false;
    }

    public override void Update()
    {
        base.Update();
    }
}
