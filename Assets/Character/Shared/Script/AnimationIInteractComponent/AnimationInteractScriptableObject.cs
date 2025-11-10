using UnityEngine;

public class AnimationInteractScriptableObject : ScriptableObject
{
    public AnimationClip animationClip;
    public float playOffsetNormalized;
    public float endOffsetNormalized;
    public static float transitionRootDrivenAnimationDuration = 0.05f;
    public AnimationInteractCharacterDetail[] animationInteractCharacterDetail;
}
