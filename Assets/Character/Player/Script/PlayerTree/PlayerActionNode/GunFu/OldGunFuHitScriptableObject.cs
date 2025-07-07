
using UnityEngine;

[CreateAssetMenu(fileName = "OldGunFuHitNode", menuName = "ScriptableObjects/GunFuNode/OldGunFuHit")]
public class OldGunFuHitScriptableObject : ScriptableObject
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

    public GotGunFuHitScriptableObject GunFu_GotHit_ScriptableObject;
}
