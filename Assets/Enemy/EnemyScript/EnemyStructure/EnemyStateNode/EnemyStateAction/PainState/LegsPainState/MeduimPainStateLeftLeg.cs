using System;
using System.Collections.Generic;
using UnityEngine;

public class MeduimPainStateLeftLeg : EnemyPainStateNodeLeaf
{
    public MeduimPainStateLeftLeg(Enemy enemy,Func<bool> preProduction,Animator animator) : base(enemy,preProduction, animator)
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
