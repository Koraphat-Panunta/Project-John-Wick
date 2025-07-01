
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunFuSingleExecute", menuName = "ScriptableObjects/GunFuNode/GunFuExecute")]
public class GunFuExecute_Single_ScriptableObject : ScriptableObject
{
    public string stateName;
    public AnimationClip clip;

    [Range(0, 1)]
    public float warpingPhaseTimeNormalized;
    [Range(0, 1)]
    public float executeTimeNormalized;

    [Range(-10, 10)]
    public float playerForwardRelativePosition;
    [Range(-10, 10)]
    public float playerRightwardRelativePosition;
    [Range(0, 360)]
    public float playerRotationRelative;

    [Range(-10, 10)]
    public float opponentForwardRelative;
    [Range(-10,10)]
    public float opponentRightwardRelative;
    [Range(0,360)]
    public float opponentRotationRelative;

    [SerializeField] public List<float> firingTimingNormalized;
}
