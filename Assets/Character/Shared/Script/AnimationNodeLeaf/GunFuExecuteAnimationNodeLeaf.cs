using System;
using UnityEngine;

public class GunFuExecuteAnimationNodeLeaf : AnimationNodeLeaf
{
    private IGunFuAble gunFuAble;
    private Animator animator;
    public IGunFuExecuteNodeLeaf gunFuExecuteNodeLeaf 
    {
        get 
        {
            if (gunFuAble.curGunFuNode != null
                && gunFuAble.curGunFuNode is IGunFuExecuteNodeLeaf executeNodeLeaf)
            {
                return executeNodeLeaf;
            }
            else
                return null;
        }
    }
    public GunFuExecuteAnimationNodeLeaf(Func<bool> preCondition, IGunFuAble gunFuAble,Animator animator) : base(preCondition)
    {
        this.gunFuAble = gunFuAble;
        this.animator = animator;
    }
    public override void Enter()
    {
        animator.CrossFade(gunFuExecuteNodeLeaf._stateName,AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration ,0,gunFuExecuteNodeLeaf._gunFuExecuteInteractSCRP.enterNormalizedTime);
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

    public override bool Precondition()
    {
        if(this.gunFuExecuteNodeLeaf == null)
            return false;

        return base.Precondition();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
