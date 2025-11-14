using UnityEngine;

[System.Serializable] 
public class AnimationInteractCharacterDetail 
{
    public string subjectName;

    public Vector2 offsetPositionFormAnchor;
    public float RotationRelative;

    public float beginWarpingNormalizedTime_BasedOnMainSubject;
    public float finishWarpingNormalizedTime_BasedOnMainSubject;
    public float beginPlayAnimationNormalizedTime_BasedOnMainSubject;
    public float enterAnimationOffsetNormalizedTime;
    public float endAnimationNormalizedTime;
}
