using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunFu_GotInteract_NodeLeaf : EnemyStateLeafNode, IGunFuAttackedAbleNode
{
    
    public GunFu_GotInteract_NodeLeaf(Enemy enemy) : base(enemy)
    {
    }

    public float _exitTime_Normalized { get; set; }
    public float _timer { get; set ; }
    public virtual bool _isExit { get; set; }
    public AnimationClip _animationClip { get; set; }

    public override void Enter()
    {
        _timer = 0;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        _timer += Time.deltaTime;
        base.Update();
    }
}
