using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunFu_GotInteract_NodeLeaf : EnemyStateLeafNode, IGotGunFuAttackNode
{
    
    public GunFu_GotInteract_NodeLeaf(Enemy enemy,Func<bool> preCondition) : base(enemy,preCondition)
    {
    }

    public float _exitTime_Normalized { get; set; }
    public float _timer { get; set ; }
    public AnimationClip _animationClip { get; set; }

    public override bool IsReset()
    {
        if(IsComplete())
            return true;
        
        if(enemy.isDead)
            return true;

        return false;
    }
    public override void Enter()
    {
        _timer = 0;
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
        _timer += Time.deltaTime;

        base.UpdateNode();
    }
}
