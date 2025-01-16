using System;
using System.Collections.Generic;
using UnityEngine;

public class Hit1GunFuNode : GunFuNodeLeaf
{
    public Hit1GunFuNode(Player player,AnimationClip animationClip) : base(player,animationClip)
    {

    }

    public override List<PlayerNode> childNode { get => base.childNode; set => base.childNode = value; }
    public override bool isTransitionAble { get ; set ; }
    public override float exitTime { get => base.gunFuAnimationClip.length; }
    public override float transitionTime { get => base.gunFuAnimationClip.length*0.5f; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    protected Vector3 targetPos;
   
    public override void Enter()
    {
         
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override bool PreCondition()
    {
        if(player.triggerGunFu)
            return true;

        return false;
    }

    
}
