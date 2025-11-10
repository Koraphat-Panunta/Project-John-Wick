using UnityEngine;

[System.Serializable] 
public struct AnimationInteractCharacterDetail 
{
    public string subjectName;

    public Vector2 offsetPositionFormAnchor;
    public float RotationRelative;

    public float beginWarpingNormalizedTime_BasedOnMainSubject;
    public float finishWarpingNormalizedTime_BasedOnMainSubject;
    public float beginPlayAnimationNormalizedTime_BasedOnMainSubject;
    public Vector2 onSyncAnimationWithBasedOnMainSubjectNormalizedTime;//X : MainSubjectAnimationNormalized , Y : SecondSubjectAnimationNormalized
    public float endAnimationNormalizedTime;
}
