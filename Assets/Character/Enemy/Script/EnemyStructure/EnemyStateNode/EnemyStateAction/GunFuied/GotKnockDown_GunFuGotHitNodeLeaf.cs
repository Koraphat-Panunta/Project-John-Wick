using System;
using UnityEngine;

public class GotKnockDown_GunFuGotHitNodeLeaf : GotGunFuHitNodeLeaf
{
    public GotKnockDown_GunFuGotHitNodeLeaf(Enemy enemy,Func<bool> preCondition, GotGunFuHitScriptableObject gunFu_GotHit_ScriptableObject) : base(enemy,preCondition, gunFu_GotHit_ScriptableObject)
    {
    }

    public override void Enter()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.animationDrivenMotionState);
        animator.CrossFade(gotHitstateName, 0.005f, 0, 0);

        enemy.NotifyObserver(enemy, this);

        base.Enter();
    }

    public override void Exit()
    {
        enemy._posture = 0;
        enemy._isPainTrigger = true;
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
        if (IsComplete())
            enemy.enemyStateManagerNode.nodeManagerBehavior.ChangeNodeManual(enemy.enemyStateManagerNode, (enemy.enemyStateManagerNode as EnemyStateManagerNode).fallDown_EnemyState_NodeLeaf);
    }
}
