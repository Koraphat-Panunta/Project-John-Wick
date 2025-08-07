using UnityEngine;

public partial class Enemy : IStaggerAble
{
    public bool isStagger 
    {
        get 
        {

            if (( staggerGauge <= 0
                || _isFallDown)
                && (isDead == false))
                return true;
            return false;
        }
    }
    public float staggerGauge { get; set; }

    [SerializeField] private float _maxStaggerGauge;
    public float maxStaggerGauge => _maxStaggerGauge;
    public RecoveryStaggerNodeLeaf recoveryStaggerNodeLeaf { get; set; }
}
