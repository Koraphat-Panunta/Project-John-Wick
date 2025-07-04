using System;
using UnityEngine;

public class GotHit1_GunFuGotHitNodeLeaf : GunFu_GotHit_NodeLeaf
{
    public GotHit1_GunFuGotHitNodeLeaf(Enemy enemy,Func<bool> preCondition, GunFu_GotHit_ScriptableObject gunFu_GotHit_ScriptableObject) : base(enemy, preCondition, gunFu_GotHit_ScriptableObject)
    {
    }

    public override void Enter()
    {
        animator.CrossFade(stateName,0.005f, 0,0);
        enemy.NotifyObserver(enemy, this);

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
