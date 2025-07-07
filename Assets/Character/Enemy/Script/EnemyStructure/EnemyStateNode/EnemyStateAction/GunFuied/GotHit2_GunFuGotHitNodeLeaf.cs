using System;
using UnityEngine;

public class GotHit2_GunFuGotHitNodeLeaf : GotGunFuHitNodeLeaf
{
    public GotHit2_GunFuGotHitNodeLeaf(Enemy enemy,Func<bool> preCondition, GotGunFuHitScriptableObject gunFu_GotHit_ScriptableObject) : base(enemy,preCondition, gunFu_GotHit_ScriptableObject)
    {
    }

    public override void Enter()
    {
        animator.CrossFade(gotHitstateName, 0.005f, 0,0);
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
