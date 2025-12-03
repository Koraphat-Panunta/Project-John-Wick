using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SetConstraintWeightNodeLeaf : AnimationConstrainNodeLeaf
{
    protected IConstraintManager constraintManager { get; set; }
    protected float speedChangeWeight { get; set; }
    protected float targetWeight { get; set; }
    protected float enterWeight { get; set; }

    public SetConstraintWeightNodeLeaf(Func<bool> precondition, IConstraintManager constraintManager, float speedSetWeight, float targetWeight) 
        : this (
              precondition
              ,constraintManager
              ,speedSetWeight
              ,0
              ,targetWeight
              )
    {


    }
    public SetConstraintWeightNodeLeaf(Func<bool> precondition, IConstraintManager constraintManager, float speedSetWeight, float enterWeight, float targetWeight) :
        base(precondition)
    {
        this.constraintManager = constraintManager;
        this.speedChangeWeight = speedSetWeight;
        this.enterWeight = enterWeight;
        this.targetWeight = targetWeight;
    }
    public override void Enter()
    {
            constraintManager.SetWeight(enterWeight);
        base.Enter();
    }
    public override void UpdateNode()
    {
        this.constraintManager.SetWeight(Mathf.MoveTowards(this.constraintManager.GetWeight(), Mathf.Clamp01(this.targetWeight), this.speedChangeWeight * Time.deltaTime));
        base.UpdateNode();
    }

    public void SetSpeedChangeWeight(float speedChangeWeight) => this.speedChangeWeight = speedChangeWeight;
    public float GetSpeedChangeWeight() => this.speedChangeWeight;
}
