using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationTriggerEventSCRP", menuName = "ScriptableObjects/AnimationTriggerEventSCRP/AnimationTriggerEventSCRP")]
public class AnimationTriggerEventSCRP : ScriptableObject
{
    public AnimationClip clip;
    [Range(0, 1)]
    public float enterNormalizedTime;
    [Range(0, 1)]
    public float triggerNormalizedTime;
    public float[] triggerTimerEventNormalized;
    [Range(0, 1)]
    public float endNormalizedTime;
}
