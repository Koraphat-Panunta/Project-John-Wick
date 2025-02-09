using System;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class LightPainStateFrontBody : EnemyPainStateNodeLeaf
{

    public LightPainStateFrontBody(Enemy enemy, Func<bool> preCondition, Animator animator) : base(enemy, preCondition, animator)
    {
        painDuration = enemy._painDurScrp.bodyFront_LightHit;
        painPart = IPainState.PainPart.BodyFornt;
    }

    public override float painDuration { get; set; }
    public override IPainState.PainPart painPart { get; set; }


    protected override string stateName => "BodyFont_Light";

    public override void Enter()
    {
      
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
