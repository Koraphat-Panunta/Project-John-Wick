using System;
using System.Collections.Generic;
using UnityEngine;

public class Stand_Move_Idle_AnimationNode : AnimationNodeLeaf
{

    public Stand_Move_Idle_AnimationNode(EnemyAnimation enemyAnimation) : base(enemyAnimation)
    {

    }

    public override List<AnimationNode> childNode { get ; set ; }
    protected override Func<bool> preCondidtion { get ; set ; }
    

    public override void FixedUpdate()
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override bool IsReset()
    {
        return true;
    }

    public override bool PreCondition()
    {
        return true;
    }

    public override void Update()
    {

    }
}
