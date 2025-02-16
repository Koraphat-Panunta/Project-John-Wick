using System;
using System.Collections.Generic;
using UnityEngine;

public class HeavyPainStateBackBody : EnemyPainStateNodeLeaf
{
    public HeavyPainStateBackBody(Enemy enemy, Func<bool> preCondition, Animator animator) : base(enemy, preCondition, animator)
    {
        painDuration = enemy._painDurScrp.bodyBack_HeavyHit;
        painPart = IPainStateAble.PainPart.BodyBack;
    }


    public override float painDuration { get; set; }
    public override IPainStateAble.PainPart painPart { get; set; }


    protected override string stateName => "BodyBack_Heavy";
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
