using System;
using System.Collections.Generic;
using UnityEngine;

public class HeavyPainStateLeftArmNode : EnemyPainStateNodeLeaf
{

    public HeavyPainStateLeftArmNode(Enemy enemy,Func<bool> preCondition,Animator animator) : base(enemy,preCondition,animator)
    {
        painDuration = enemy._painDurScrp.armLeft_HeavyHit;
        painPart = IPainState.PainPart.ArmLeft;
    }

    public override float painDuration { get; set ; }
    public override IPainState.PainPart painPart { get ; set ; }

    protected override string stateName { get => "LeftArm_Heavy"; }

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
