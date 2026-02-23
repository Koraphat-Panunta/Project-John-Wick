using UnityEngine;

public interface IStaggerAble 
{
    public bool isStagger { get; }
    public float staggerGauge { get; set; }
    public float maxStaggerGauge { get; }
    public float SetStaggerGauge(float value) => staggerGauge = value;
    public Character _character { get; }
}
