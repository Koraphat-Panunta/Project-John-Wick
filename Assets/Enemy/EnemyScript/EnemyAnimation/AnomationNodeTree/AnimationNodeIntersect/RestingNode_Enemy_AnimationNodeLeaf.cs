using System;
using UnityEngine;

public class RestingNode_Enemy_AnimationNodeLeaf : EnemyStateLeafNode
{
    private Animator _animator;
    public RestingNode_Enemy_AnimationNodeLeaf(Enemy enemy,Animator animator, Func<bool> preCondition) : base(enemy, preCondition)
    {
        _animator = animator;
    }
    public override void UpdateNode()
    {
        _animator.SetLayerWeight(1, 0);
        base.UpdateNode();
    }
}
