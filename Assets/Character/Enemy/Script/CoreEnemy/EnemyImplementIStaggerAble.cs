using UnityEngine;

public partial class Enemy : IStaggerAble
{
    public bool isStagger 
    {
        get 
        {
            if(staggerGauge <= 0
                || _isFallDown)
                return true;
            return false;
        }
    }
    public float staggerGauge { get; set; }
}
