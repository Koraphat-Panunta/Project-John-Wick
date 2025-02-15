using System;
using System.Collections.Generic;
using UnityEngine;

public class LightPainStateHeadNode : EnemyPainStateNodeLeaf
{
    public LightPainStateHeadNode(Enemy enemy,Func<bool> preCondition, Animator animator) : base(enemy, preCondition, animator)
    {
        painDuration = enemy._painDurScrp.head_LightHit;
        painPart = IPainStateAble.PainPart.Head;
    }

   
    public override float painDuration { get; set; }
    public override IPainStateAble.PainPart painPart { get; set; }


    protected override string stateName => "HeadHit_Light";

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

}
