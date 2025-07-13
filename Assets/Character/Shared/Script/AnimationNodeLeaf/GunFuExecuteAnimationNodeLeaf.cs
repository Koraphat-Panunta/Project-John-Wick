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
        Debug.Log("gunFuExecuteNodeLeaf._stateName = "+ gunFuExecuteNodeLeaf._stateName);
        animator.CrossFade(gunFuExecuteNodeLeaf._stateName,0.1f,0,gunFuExecuteNodeLeaf._gunFuExecuteScriptableObject.executeAnimationOffset);
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
