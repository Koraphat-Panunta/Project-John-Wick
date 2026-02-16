using UnityEngine;

[CreateAssetMenu(fileName = "PlayPoseAnimationScriptableObject", menuName = "ScriptableObjects/Animation/PoseAnimationSCRP")]
public class PlayPoseAnimationScriptableObject : ScriptableObject
{
    public bool isLoop;
    public float isLoopAtNormalized;
    public float duration;
    public float startNormalized;

    public AnimationCurve animationCurve;
}
