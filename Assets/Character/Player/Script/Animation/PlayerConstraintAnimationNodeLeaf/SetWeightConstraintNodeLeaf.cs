using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SetWeightConstraintNodeLeaf : AnimationConstrainNodeLeaf
{
    protected Rig rig { get; set; }
    protected float speedSetWeight { get; set; }
    protected float targetWeight { get; set; }
    public SetWeightConstraintNodeLeaf(Func<bool> precondition,Rig rig,float speedSetWeight,float targetWeight) : base(precondition)
    {
        this.rig = rig;
        this.speedSetWeight = speedSetWeight;
        this.targetWeight = targetWeight;
    }
    public override void UpdateNode()
    {
        rig.weight = Mathf.MoveTowards(rig.weight , Mathf.Clamp01(targetWeight) , speedSetWeight * Time.deltaTime);
        base.UpdateNode();
    }
}
