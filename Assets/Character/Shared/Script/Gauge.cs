using System;
using System.Collections.Generic;
using UnityEngine;

public class Gauge 
{
   
    public float _gauge { get; protected set; }
    public float maxGauge { get; protected set; }
    private float minGauge = 0;

    public Gauge(float gauge
        , float maxGauge
        , float minGauge)
    {
        this._gauge = gauge;
        this.maxGauge = maxGauge;
        this.minGauge = minGauge;
    }

    public Gauge(float gauge,float maxGauge) : this(gauge, maxGauge, 0) { }

    public void SetGauge(float value)
    {
        this._gauge = Mathf.Clamp(value, this.minGauge, this.maxGauge);
    }

    public void SetMaxGauge(float value)
    {
        this.maxGauge = value;
    }

    public void SetMinGauge(float value)
    {
        this.minGauge = value;
    }

    public void AddGauge(float value)
    {
        this.SetGauge( this._gauge + value );
    }

    public bool IsFull() => this._gauge >= this.maxGauge;

    public bool CompareValue_Greater_Equal_ThanGauge(float value)
    {
        if(this._gauge >= value)
            return true;
        else return false;
    }

}
