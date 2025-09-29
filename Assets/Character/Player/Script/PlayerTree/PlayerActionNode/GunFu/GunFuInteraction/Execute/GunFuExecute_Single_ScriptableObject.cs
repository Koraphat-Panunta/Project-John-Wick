
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunFuSingleExecute", menuName = "ScriptableObjects/GunFu/GunFu_Single_Execute")]
public class GunFuExecute_Single_ScriptableObject : GunFuExecuteScriptableObject
{

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
    [Range(0, 6)]
    public float slowMotionDurarion;

    [SerializeField] public AnimationCurve slowMotionCurve;


    [SerializeField] public List<float> firingTimingNormalized;

}
