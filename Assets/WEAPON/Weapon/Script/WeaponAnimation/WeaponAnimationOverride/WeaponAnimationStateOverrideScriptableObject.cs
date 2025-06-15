using UnityEngine;


public abstract class WeaponAnimationStateOverrideScriptableObject : ScriptableObject
{
    [SerializeField] public AnimationClip idleLowReady;
    [SerializeField] public AnimationClip moveLowReady;

    [SerializeField] public AnimationClip ADS_20;
    [SerializeField] public AnimationClip CAR_20;

    [SerializeField] public AnimationClip ADS_40;
    [SerializeField] public AnimationClip CAR_40;

    [SerializeField] public AnimationClip ADS_60;
    [SerializeField] public AnimationClip CAR_60;

    [SerializeField] public AnimationClip ADS_80;
    [SerializeField] public AnimationClip CAR_80;

    [SerializeField] public AnimationClip ADS_100;
    [SerializeField] public AnimationClip CAR_100;

    [SerializeField] public AnimationClip ADS_RecoilKick;
    [SerializeField] public AnimationClip CAR_RecoilKick;

    [SerializeField] public AnimationClip TacticalSprint_LowReadySway;
    [SerializeField] public AnimationClip TacticalSprint_HighReadySway;
}
