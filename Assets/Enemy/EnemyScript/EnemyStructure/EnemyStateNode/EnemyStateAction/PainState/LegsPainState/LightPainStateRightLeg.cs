using System;
using System.Collections.Generic;
using UnityEngine;

public class LightPainStateRightLeg : EnemyPainStateNodeLeaf
{

    public LightPainStateRightLeg(Enemy enemy,Func<bool> preCondition, Animator animator) : base(enemy, preCondition, animator)
    {
        painDuration = enemy._painDurScrp.legRight_LightHit;
        painPart = IPainState.PainPart.LegRight;
    }
    
    public override float painDuration { get; set; }
    public override IPainState.PainPart painPart { get; set; }

    protected override string stateName => "RightLeg_Light";

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
