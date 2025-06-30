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
    public Transform beginAnchorInterpolate;
    [Range(-10, 10)]
    public float playerForwardRelativePosition;
    [Range(-10, 10)]
    public float playerRightwardRelativePosition;
    [Range(0, 360)]
    public float playerRotationRelative;
}
