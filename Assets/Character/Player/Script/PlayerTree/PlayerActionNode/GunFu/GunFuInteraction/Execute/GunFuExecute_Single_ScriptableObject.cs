
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunFuSingleExecute", menuName = "ScriptableObjects/GunFu/GunFuExecute")]
public class GunFuExecute_Single_ScriptableObject : ScriptableObject
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
    [Range(-10, 10)]
    public float playerForwardRelativePosition;
    [Range(-10, 10)]
    public float playerRightwardRelativePosition;
    [Range(0, 360)]
    public float playerRotationRelative;

    [Range(0, 1)]
    public float opponentAnimationOffset;
    [Range(-10, 10)]
    public float opponentForwardRelative;
    [Range(-10,10)]
    public float opponentRightwardRelative;
    [Range(0,360)]
    public float opponentRotationRelative;

    [Range(0, 1)]
    public float slowMotionTriggerNormailzed;

    [SerializeField] public List<float> firingTimingNormalized;

}
