using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationNodeSelector : PlayerAnimationNode
{
    public PlayerAnimationNodeSelector(PlayerAnimationManager playerAnimationManager, Animator animator, Func<bool> preCondition) : base(playerAnimationManager, animator)
    {
        this.preCondidtion = preCondition;
        childNode = new List<PlayerAnimationNode>();
    }

    public override List<PlayerAnimationNode> childNode { get ; set; }
    protected override Func<bool> preCondidtion { get; set ; }

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
