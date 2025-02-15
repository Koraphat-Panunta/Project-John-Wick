using System;
using System.Collections.Generic;
using UnityEngine;

public class LightPainStateLeftLeg : EnemyPainStateNodeLeaf
{
    public LightPainStateLeftLeg(Enemy enemy, Func<bool> preCondition, Animator animator) : base(enemy, preCondition, animator)
    {
        painDuration = enemy._painDurScrp.legLeft_LightHit;
        painPart = IPainStateAble.PainPart.LegLeft;
    }

    
    public override float painDuration { get; set; }
    public override IPainStateAble.PainPart painPart { get; set; }


    protected override string stateName => "LeftLeg_Light";

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
