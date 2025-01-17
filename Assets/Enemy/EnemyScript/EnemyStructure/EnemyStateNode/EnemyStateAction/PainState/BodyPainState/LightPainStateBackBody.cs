using System;
using System.Collections.Generic;
using UnityEngine;

public class LightPainStateBackBody : EnemyPainStateNodeLeaf
{
    public LightPainStateBackBody(Enemy enemy) : base(enemy)
    {
        painDuration = enemy._painDurScrp.bodyBack_LightHit;
        painPart = IPainState.PainPart.BodyBack;
    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    public override float painDuration { get; set; }
    public override IPainState.PainPart painPart { get; set; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

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
        if (enemy.isDead)
            return true;

        if (time >= painDuration)
            return true;

        if (enemy._isPainTrigger)
            return true;

        return false;
    }

    public override void Update()
    {
        base.Update();
    }
}
