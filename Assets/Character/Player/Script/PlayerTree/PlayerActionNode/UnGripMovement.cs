using UnityEngine;

[CreateAssetMenu(fileName = "UnGripMovement", menuName = "ScriptableObjects/Movement/UnGripMovement")]
public class UnGripMovement : ScriptableObject
{
    public AnimationTriggerEventSCRP animationTriggerEvent;
    public AnimationCurve gripCurve;
}
