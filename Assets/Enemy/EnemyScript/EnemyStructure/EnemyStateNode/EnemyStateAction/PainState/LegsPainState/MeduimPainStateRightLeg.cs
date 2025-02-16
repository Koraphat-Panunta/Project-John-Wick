using System;
using System.Collections.Generic;
using UnityEngine;

public class MeduimPainStateRightLeg : EnemyPainStateNodeLeaf
{
    public MeduimPainStateRightLeg(Enemy enemy,Func<bool> preCondition,Animator animator) : base(enemy,preCondition,animator)
    {
    }

   
    public override float painDuration { get; set; }
    public override IPainStateAble.PainPart painPart { get; set; }
  

    protected override string stateName => throw new NotImplementedException();

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
