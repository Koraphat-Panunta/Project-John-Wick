using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GunFuHitNode", menuName = "ScriptableObjects/GunFuNode/GunFuHit")]
public class GunFuHitScriptableObject : ScriptableObject
{
    public AnimationClip animationClip_GunFuHits;
    public string gunFuHitStateName;
    [SerializeField] public List<float> hitTimesNormalized;
    [SerializeField] public List<float> hitStopDuration;
    [SerializeField] public List<float> hitResetDuration;
    [Range(0, 1)]
    public float animationGunFuHitOffset;
    [Range(0,1)]
    public float ExitTime_Normalized;
    [Range(0, 1)]
    public float TransitionAbleTime_Normalized;

    public AnimationClip animationClip_GotHits;
    public List<float> gotHitEnterOffset;
    public string gotHitStateNmae;


}
