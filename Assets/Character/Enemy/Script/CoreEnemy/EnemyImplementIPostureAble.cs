using UnityEngine;

public partial class Enemy : IPostureAble
{
    public float _maxPosture { get ; set ; }
    public float _posture { get ; set; }

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
}
