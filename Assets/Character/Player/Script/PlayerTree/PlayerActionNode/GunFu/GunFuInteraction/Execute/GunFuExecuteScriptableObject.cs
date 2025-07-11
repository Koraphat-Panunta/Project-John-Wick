using UnityEngine;

public abstract class GunFuExecuteScriptableObject : ScriptableObject
{
    public string gunFuStateName;
    public string gotGunFuStateName;

    public AnimationClip executeClip;
    public AnimationClip gotExecuteClip;

    [Range(0, 1)]
    public float warpingPhaseTimeNormalized;
    [Range(0, 1)]
    public float executeTimeNormalized;
    [Range(0, 1)]
    public float executeAnimationExitNormarlized;
    [Range(0, 1)]
    public float executeAnimationOffset;
}
