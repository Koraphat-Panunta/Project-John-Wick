using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RestAnimationConstrainNodeLeaf : AnimationConstrainNodeLeaf
{
    private Rig rig;
    public RestAnimationConstrainNodeLeaf(Rig rig,Func<bool> precondition) : base(precondition)
    {
        this.rig = rig;
    }
    public override void Enter()
    {
        this.rig.weight = 0;
        base.Enter();
    }
    public override void Exit()
    {
        this.rig.weight = 1;
        base.Exit();
    }
}
