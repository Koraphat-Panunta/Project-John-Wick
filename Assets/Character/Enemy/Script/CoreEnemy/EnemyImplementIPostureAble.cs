using UnityEngine;

public partial class Enemy : IPostureAble
{
    public float _maxPosture 
    {
        get => this.postureGauge.maxGauge; 
        set => this.postureGauge.SetMaxGauge(value); 
    }
    public float _posture 
    {
        get => this.postureGauge._gauge; 
        set => this.postureGauge.SetGauge(value);
    }

    public Gauge postureGauge;

    [Range(0,100)]
    [SerializeField] public float lightPosture;

    [Range(0,100)]
    [SerializeField] public float mediumPosture;

    [Range(0,100)]
    [SerializeField] public float heavyPosture;

    public EnemyPosturePainStatePhase getPosturePainPhase
    {
        get 
        {
            if (_posture >= _maxPosture)
                return EnemyPosturePainStatePhase.None;
            else if (_posture > lightPosture)
                return EnemyPosturePainStatePhase.Flinch;
            else if (_posture > mediumPosture)
                return EnemyPosturePainStatePhase.MiniPainState;
            else if (_posture > heavyPosture)
                return EnemyPosturePainStatePhase.MediumPainState;
            else //_posture > 0
                return EnemyPosturePainStatePhase.HeavyPainState;
        }
    }

    public enum EnemyPosturePainStatePhase
    {
        None,
        Flinch,
        MiniPainState,
        MediumPainState,
        HeavyPainState
    }

    public void TakePostureDamaged(float postureDamage)
    {
        if(this._posture > 0)
            this._posture -= postureDamage; 

    }
}
