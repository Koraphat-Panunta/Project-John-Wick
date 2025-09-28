using System;
using UnityEngine;

public class TimeScaleSetNodeLeaf : TimeNodeLeaf
{
    protected float enterTimeScale;
    protected float exitTimeScale;
    public TimeScaleSetNodeLeaf(Func<bool> preCondition,TimeControlManager timeControlManager,float enterTimeScale,float exitTimeScale) : base(preCondition,timeControlManager)
    {
        this.enterTimeScale = enterTimeScale;
        this.exitTimeScale = exitTimeScale;
    }
    public override void Enter()
    {
        Time.timeScale = enterTimeScale;
        Time.fixedDeltaTime = TimeControlManager.fixDeltaTimeOnSlowMotion;
        base.Enter();
    }
    public override void Exit()
    {
        Time.timeScale = exitTimeScale;
        Time.fixedDeltaTime = TimeControlManager.fixDeltaTimeDefault;
        base.Exit();
    }
    public override void FixedUpdateNode()
    {
        
    }

    public override void UpdateNode()
    {
        
    }
}
