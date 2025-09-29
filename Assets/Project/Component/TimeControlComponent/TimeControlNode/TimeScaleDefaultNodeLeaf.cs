using System;
using UnityEngine;

public class TimeScaleDefaultNodeLeaf : TimeNodeLeaf
{
    protected float enterTimeScale;
    protected float exitTimeScale;
    public TimeScaleDefaultNodeLeaf(Func<bool> preCondition,TimeControlManager timeControlManager,float enterTimeScale,float exitTimeScale) : base(preCondition,timeControlManager)
    {
        this.enterTimeScale = enterTimeScale;
        this.exitTimeScale = exitTimeScale;
    }
    public override void Enter()
    {
        Time.timeScale = enterTimeScale;

        base.Enter();
    }
    public override void Exit()
    {

        base.Exit();
    }
    public override void FixedUpdateNode()
    {
        
    }

    public override void UpdateNode()
    {
        
    }
}
