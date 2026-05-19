using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "GunFuHitScrp", menuName = "ScriptableObjects/GunFu/GunFuHit")]
public class GunFuHitScriptableObject : AnimationTriggerEventSCRP
{
    public GunFuHitDetail[] gunFuHitDetail;
    public AudioAnimationTriggerEvent[] audiotriggerEvents;
}

[Serializable]
public struct GunFuHitDetail
{
    public string gunFuHitStateName;

    [Range(0, 100)]
    public float postureHitDamage;

    [Range(0, 100)]
    public float hpHitDamage;

    [Range(0, 100)]
    public float stuntingTime;

    [Range(0, 10)]
    public float attackRange;

    [Range(0, 100)]
    public float attackMoveVelocity;

    [Range(0, 500)]
    public float attackRotateVelocity;

    public AnimationCurve hitSlowMotionCurve;
    public AnimationCurve warpingMovementCurve;

    public Vector2 warpingTime;

    [SerializeField] public Vector2 hitDirRotOffset;
    [SerializeField] public Vector3 hitDirPoseAnimOffset;
    [SerializeField] public float hitPushForce;
    [SerializeField] public float hitPushAnimConstrainForce;
    [SerializeField] public float hitStopDuration;
    [SerializeField] public float hitResetDuration;

    [Range(0, 10)]
    public float attackVolumeRaduis;

    [Range(-10, 10)]
    public float attackVolumeForward;


    [Range(-10, 10)]
    public float attackVolumeUpward;

    [Range(-10, 10)]
    public float attackVolumeRightward;
}


