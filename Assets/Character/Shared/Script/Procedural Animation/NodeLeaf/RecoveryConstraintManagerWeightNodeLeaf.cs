using System;
using UnityEngine;

public class RecoveryConstraintManagerWeightNodeLeaf : AnimationConstrainNodeLeaf
{
    protected IConstraintManager constraintManager;
    protected float recoverySpeed;
    public RecoveryConstraintManagerWeightNodeLeaf(Func<bool> precondition,IConstraintManager constraintManager,float recoverySpeed) : base(precondition)
    {
        this.constraintManager = constraintManager;
        this.recoverySpeed = recoverySpeed;
    }
    public override void UpdateNode()
    {
        constraintManager.SetWeight(Mathf.Clamp01(constraintManager.GetWeight() - recoverySpeed*Time.deltaTime));
        base.UpdateNode();
    }

}
