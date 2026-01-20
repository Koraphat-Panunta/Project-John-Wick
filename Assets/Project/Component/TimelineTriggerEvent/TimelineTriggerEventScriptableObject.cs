using UnityEngine;

[CreateAssetMenu(fileName = "TimelineTriggerEventScriptableObject", menuName = "ScriptableObjects/TimelineTriggerEvent/TimelineTriggerEventSCRP")]
public class TimelineTriggerEventScriptableObject : ScriptableObject
{
    public float timeDuration;
    public AnimationTriggerEventDetail[] triggerEventDetail;
}
