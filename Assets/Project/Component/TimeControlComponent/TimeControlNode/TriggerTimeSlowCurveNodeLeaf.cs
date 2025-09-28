using System;
using UnityEngine;

public class TriggerTimeSlowCurveNodeLeaf : TimeNodeLeaf
{
    protected float duration;
    protected float timer;
    protected AnimationCurve timeCurve;
    public TriggerTimeSlowCurveNodeLeaf(Func<bool> preCondition, TimeControlManager timeControlManager,float duration,AnimationCurve timeCurve) : base(preCondition, timeControlManager)
    {
        this.duration = duration;
        this.timeCurve = timeCurve;
    }
    public override void Enter()
    {
        this.timer = 0;
        Time.fixedDeltaTime = TimeControlManager.fixDeltaTimeOnSlowMotion;
        base.Enter();
    }
    public override void Exit()
    {
        Time.fixedDeltaTime = TimeControlManager.fixDeltaTimeDefault;
        base.Exit();
    }
    public override bool IsReset()
    {
        return base.IsComplete();
    }
    public override void FixedUpdateNode()
    {
       
    }

    public override void UpdateNode()
    {
        this.timer += Time.unscaledDeltaTime;
        float normalized = Mathf.Clamp01(this.timer / this.duration);

        float timeScale = (timeCurve != null)
            ? timeCurve.Evaluate(normalized)
            : normalized;

        Time.timeScale = timeScale;

        if(this.timer > this.duration)
            base.isComplete = true;

    }
}
