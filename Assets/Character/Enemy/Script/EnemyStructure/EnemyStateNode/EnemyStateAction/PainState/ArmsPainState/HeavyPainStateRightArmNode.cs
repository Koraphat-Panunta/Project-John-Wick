using System;
using System.Collections.Generic;
using UnityEngine;

public class HeavyPainStateRightArmNode : EnemyPainStateNodeLeaf
{

    public HeavyPainStateRightArmNode(Enemy enemy, Func<bool> preCondition, Animator animator) : base(enemy, preCondition, animator)
    {
        painDuration = enemy._painDurScrp.armRight_HeavyHit;
        painPart = IPainStateAble.PainPart.ArmRight;
    }


    public override float painDuration { get; set; }
    public override IPainStateAble.PainPart painPart { get; set; }


    protected override string stateName => "RightArm_Heavy";

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
