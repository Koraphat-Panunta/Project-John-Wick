
using UnityEngine;

[CreateAssetMenu(fileName = "GunFuNode", menuName = "ScriptableObjects/GunFuNode")]
public class GunFuHitNodeScriptableObject : ScriptableObject
{
    public AnimationClip animationClip;
    public float ExitTime_Normalized;
    public float TransitionAbleTime_Normalized;
    public float HitAbleTime_Normalized;
    public float EndHitAbleTime_Normalized;

    [Range(0.001f, 1)]
    public float HitStopDuration;
    [Range(0.001f,5)]
    public float HitResetDuration;

    public GunFu_GotHit_ScriptableObject GunFu_GotHit_ScriptableObject;
}
