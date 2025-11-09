using UnityEngine;

public class AnimationInteractScriptableObject : ScriptableObject
{
    public AnimationClip animationClip;
    [Range(0.001f, 1)]
    public float transitionRootDrivenAnimationDuration;
    public AnimationInteractCharacterDetail[] animationInteractCharacterDetail;
}
