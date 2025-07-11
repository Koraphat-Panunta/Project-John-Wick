using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunFuSingleExecute", menuName = "ScriptableObjects/GunFu/GunFu_OnGround_Single_Execute")]
public class GunFuExecute_OnGround_Single_ScriptableObject : GunFuExecuteScriptableObject
{

    [Range(0, 1)]
    public float slowMotionTriggerNormailzed;

    [Range(-10, 10)]
    public float playerForwardRelativePosition;
    [Range(-10, 10)]
    public float playerRightwardRelativePosition;
    [Range(0, 360)]
    public float playerRotationRelative;

    [SerializeField] public List<float> firingTimingNormalized;
}
