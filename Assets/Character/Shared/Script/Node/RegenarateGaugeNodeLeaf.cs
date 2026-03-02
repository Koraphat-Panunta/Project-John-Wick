using System;
using UnityEngine;

public class RegenarateGaugeNodeLeaf : RestNodeLeaf
{

    public RegenarateGaugeNodeLeaf
        (
        Func<bool> preCondition
        , Gauge gauge
        ,float limitRegenValue
        , float regenSpeed
        ) : base(preCondition)
    {
        this.gauge = gauge;
        this.limitRegenValue = limitRegenValue;
        this.regenSpeed = regenSpeed;
    }

    protected Gauge gauge;
    protected float beenDelay;
    protected float regenSpeed;
    protected float limitRegenValue;

    public bool isDelay => this.beenDelay > 0? true : false;

    public override void Enter()
    {
        //this.Notify();
        base.Enter();
    }

    public override void Exit()
    {
        //this.Notify();
        base.Exit();
    }

    public override void FixedUpdateNode()
    {

        if(this.beenDelay > 0)
        {
            this.beenDelay -= Time.fixedDeltaTime;
            return;
        }

        this.gauge.SetGauge
            (
            Mathf.Clamp(this.gauge._gauge + (this.regenSpeed * Time.fixedDeltaTime), this.gauge._gauge , this.limitRegenValue)
            );
        //this.Notify();

        base.FixedUpdateNode();
    }

    public void SetDelay(float delay) => this.beenDelay = delay;
    public void SetLimitRegenValue(float value) => this.limitRegenValue = value;
    public void SetRegenSpeed(float value) => this.regenSpeed = value;


    protected INodeNotifyBackAble notifyBackAble;
    public void SubcribeNotifyBack(INodeNotifyBackAble nodeNotifyBackAble) => this.notifyBackAble = nodeNotifyBackAble;
    protected void Notify()
    {
        if(this.notifyBackAble == null)
            return;

        this.notifyBackAble.OnNotifyBack(this, this);
    }
}
