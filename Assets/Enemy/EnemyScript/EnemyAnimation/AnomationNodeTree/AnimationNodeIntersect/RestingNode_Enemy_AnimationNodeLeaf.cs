using System;
using UnityEngine;

public class RestingNode_Enemy_AnimationNodeLeaf : EnemyStateLeafNode
{
    private Animator _animator;
    public RestingNode_Enemy_AnimationNodeLeaf(Enemy enemy,Animator animator, Func<bool> preCondition, Func<bool> isReset) : base(enemy, preCondition, isReset)
    {
        _animator = animator;
    }
    public override void Update()
    {
        _animator.SetLayerWeight(1, 0);
        base.Update();
    }
}
