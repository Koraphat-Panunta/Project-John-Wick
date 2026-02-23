using UnityEngine;

public partial class Enemy : IStaggerAble
{
    public bool isStagger 
    {
        get 
        {
            if (( this.staggerGauge <= 15)
                && (isDead == false))
                return true;
            return false;
        }
    }

    public float staggerGauge { get => this.GetHP(); set { } }
    public float maxStaggerGauge => this.GetMaxHp();
    Character IStaggerAble._character { get => this ;  }
}
