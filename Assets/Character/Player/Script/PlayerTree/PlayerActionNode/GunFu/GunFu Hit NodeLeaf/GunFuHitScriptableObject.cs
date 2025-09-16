using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "GunFuHitScrp", menuName = "ScriptableObjects/GunFu/GunFuHit")]
public class GunFuHitScriptableObject : ScriptableObject
{
    public AnimationClip animationClip_GunFuHits;
    public string gunFuHitStateName;
    [SerializeField] public List<Vector2> hitTimes;
    [SerializeField] public List<float> hitPushRotationOffset;
    [SerializeField] public List<float> hitPushForce;
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

    [Range(0, 10)]
    public float attackVolumeRaduis;

    [Range(-10, 10)]
    public float attackVolumeForward;


    [Range(-10, 10)]
    public float attackVolumeUpward;

    [Range(-10, 10)]
    public float attackVolumeRightward;
}
