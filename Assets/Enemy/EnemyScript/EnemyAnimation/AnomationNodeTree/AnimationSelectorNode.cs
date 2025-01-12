using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSelectorNode : AnimationNode
{
    public AnimationSelectorNode(Func<bool> precondition,EnemyAnimation enemyAnimation) : base(enemyAnimation)
    {
        preCondidtion = precondition;
    }

    public override List<AnimationNode> childNode { get; set ; }
    protected override Func<bool> preCondidtion { get ; set ; }

    public override void FixedUpdate()
    {
    }

    public override bool IsReset()
    {
        return true;
    }

    public override bool PreCondition()
    {
        return preCondidtion.Invoke();
    }

    public override void Update()
    {
    }
}
