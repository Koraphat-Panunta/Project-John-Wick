using UnityEngine;

[CreateAssetMenu(fileName = "AnimationInteractScriptableObject", menuName = "ScriptableObjects/AnimationTriggerEventSCRP/AnimationInteractScriptableObject")]
public class AnimationInteractScriptableObject : AnimationTriggerEventSCRP
{
    public static float transitionRootDrivenAnimationDuration = 0.025f;
    public AnimationInteractCharacterDetail[] animationInteractCharacterDetail;

    public AudioAnimationTriggerEvent[] audioAnimationInteractTriggerEvents;
}

[System.Serializable]
public class AudioAnimationTriggerEvent
{
    public float normalizedTime;
    public SoundData soundData;
}
