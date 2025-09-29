using System;
using UnityEngine;

public class TriggerTimeSlowCurveNodeLeaf : TimeNodeLeaf
{
    protected float duration;
    public float timer;
    protected AnimationCurve timeCurve;
    public TriggerTimeSlowCurveNodeLeaf(Func<bool> preCondition, TimeControlManager timeControlManager) : base(preCondition, timeControlManager)
    {
    }
    public override void Enter()
    {
        Time.fixedDeltaTime = TimeControlManager.fixDeltaTimeOnSlowMotion;
        base.Enter();
    }
    public override void Exit()
    {
        Time.fixedDeltaTime = TimeControlManager.fixDeltaTimeDefault;
        base.Exit();
    }
   
    public override void FixedUpdateNode()
    {
       
    }

    public override void UpdateNode()
    {
      

        if(this.timer <= 0)
            return;

        this.timer -= Time.unscaledDeltaTime;
        float normalized = Mathf.Clamp01( (this.duration - this.timer) / this.duration);

        float timeScale = (timeCurve != null)
            ? timeCurve.Evaluate(normalized)
            : normalized;

        Time.timeScale = timeScale;

    }

    public virtual void TriggerSlowMotion(AnimationCurve timeCurve ,float duration)
    {
        this.timeCurve = timeCurve;
        this.duration = duration;
        this.timer = this.duration;
    }
    
    public virtual void StopSlowMotion()
    {
        this.timer = 0;
    }
}

