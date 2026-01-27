using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SetRigWeightNodeLeaf : AnimationConstrainNodeLeaf
{
    protected Rig rig { get; set; }
    protected float speedSetWeight { get; set; }
    protected float targetWeight { get; set; }
    protected float enterWeight { get; set; }
    protected bool isSetEnterWeight { get; set; }
    public SetRigWeightNodeLeaf(Func<bool> precondition,Rig rig,float speedSetWeight,float targetWeight) : base(precondition)
    {
        this.rig = rig;
        this.speedSetWeight = speedSetWeight;
        this.targetWeight = targetWeight;
        this.isSetEnterWeight = false;
    }
    public SetRigWeightNodeLeaf(Func<bool> precondition, Rig rig, float speedSetWeight,float enterWeight, float targetWeight) :
        this(precondition,rig,speedSetWeight,targetWeight)
    {
        this.enterWeight = enterWeight;
        this.isSetEnterWeight = true;
    }
    public override void Enter()
    {
        if(isSetEnterWeight)
           rig.weight = this.enterWeight;
        base.Enter();
    }
    public override void UpdateNode()
    {
        rig.weight = Mathf.MoveTowards(rig.weight , Mathf.Clamp01(targetWeight) , speedSetWeight * Time.deltaTime);
        base.UpdateNode();
    }
}
