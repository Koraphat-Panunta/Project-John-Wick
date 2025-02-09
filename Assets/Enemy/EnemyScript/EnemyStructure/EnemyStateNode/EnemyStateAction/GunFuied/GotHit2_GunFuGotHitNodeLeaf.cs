using System;
using UnityEngine;

public class GotHit2_GunFuGotHitNodeLeaf : GunFu_GotHit_NodeLeaf
{
    public GotHit2_GunFuGotHitNodeLeaf(Enemy enemy,Func<bool> preCondition, GunFu_GotHit_ScriptableObject gunFu_GotHit_ScriptableObject) : base(enemy,preCondition, gunFu_GotHit_ScriptableObject)
    {
    }

    public override void Enter()
    {
        animator.CrossFade(stateName, 0.005f, 0);
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuGotHit);

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

    public override bool IsReset()
    {
        if(_isExit)
            return true;

        return false;
    }

    public override void UpdateNode()
    {

        base.UpdateNode();
    }
}
