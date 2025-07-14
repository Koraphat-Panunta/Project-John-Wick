using UnityEngine;

public interface IStaggerAble 
{
    public bool isStagger { get; }
    public float staggerGauge { get; set; }
    public float maxStaggerGauge { get; }
}
