using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "GunFuHitScrp", menuName = "ScriptableObjects/GunFu/GunFuHit")]
public class GunFuHitScriptableObject : ScriptableObject
{
    public AnimationClip animationClip_GunFuHits;
    public string gunFuHitStateName;
    [SerializeField] public List<float> hitTimesNormalized;
    [SerializeField] public List<float> hitPush;
    [SerializeField] public List<float> hitStopDuration;
    [SerializeField] public List<float> hitResetDuration;
    [SerializeField] public List<AnimationCurve> hitSlowMotionCurve;
    [SerializeField] public List<float> warpKeyFrameNormalized;
    [Range(0, 1)]
    public float animationGunFuHitOffset;
    [Range(0,1)]
    public float ExitTime_Normalized;
    [Range(0, 1)]
    public float TransitionAbleTime_Normalized;

    [Range(0, 100)]
    public float staggerHitDamage;
}
