using UnityEngine;

public class AnimationInteractionDebugTest : MonoBehaviour,IInitializedAble
{
    [SerializeField] private AnimationInteractScriptableObject animationInteractScriptableObject;

    private SubjectAnimationInteract subjectAnimationInteract1;
    private SubjectAnimationInteract subjectAnimationInteract2;

    [SerializeField] private Character subject1;
    [SerializeField] private string subject1Animation;
    private Vector3 subject1_Enter_Pos;
    private Vector3 subject1_Enter_Dir;

    [SerializeField] private Character subject2;
    [SerializeField] private string subject2Animation;
    private Vector3 subject2_Enter_Pos;
    private Vector3 subject2_Enter_Dir;

    private Vector3 anchorPos;
    private Vector3 anchorDir;
    [SerializeField] private bool triggerRestart;
    public void Initialized()
    {
        this.subject1_Enter_Pos = this.subject1.transform.position;
        this.subject1_Enter_Dir = this.subject1.transform .forward;

        this.subject2_Enter_Pos = this.subject2.transform.position;
        this.subject2_Enter_Dir = this.subject2.transform .forward;

        this.anchorPos = this.subject2_Enter_Pos;
        this.anchorDir = this.subject2_Enter_Dir;

        subjectAnimationInteract1 = new SubjectAnimationInteract(
            animationInteractScriptableObject.clip
            ,animationInteractScriptableObject.enterNormalizedTime
            ,animationInteractScriptableObject.endNormalizedTime
            , animationInteractScriptableObject.animationInteractCharacterDetail[0]);

        subjectAnimationInteract1.beginPlayAnimationEvent += BeginPlayAnimation;
        subjectAnimationInteract1.finishWarpEvent += BeginInteract;

        subjectAnimationInteract1.RestartSubject(subject1, anchorPos, anchorDir);

        subjectAnimationInteract2 = new SubjectAnimationInteract(
           animationInteractScriptableObject.clip
           , animationInteractScriptableObject.enterNormalizedTime
           , animationInteractScriptableObject.endNormalizedTime
           , animationInteractScriptableObject.animationInteractCharacterDetail[1]);

        subjectAnimationInteract2.beginPlayAnimationEvent += BeginPlayAnimation;
        subjectAnimationInteract2.finishWarpEvent += BeginInteract;

        subjectAnimationInteract2.RestartSubject(subject2, anchorPos, anchorDir);
    }

    private void BeginPlayAnimation(Character character)
    {
        if(character == subject1)
            subject1.animator.CrossFade(
                subject1Animation
                ,AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration
                ,0
                ,subjectAnimationInteract1.animationInteractCharacterDetail.enterAnimationOffsetNormalizedTime);
        else if(character == subject2)
            subject2.animator.CrossFade(
                subject1Animation
                , AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration
                , 0
                , subjectAnimationInteract2.animationInteractCharacterDetail.enterAnimationOffsetNormalizedTime);
    }

    private void BeginInteract(Character character)
    {
        character.enableRootMotion = true;
    }

    public void Restart()
    {
        subject1.enableRootMotion = false;
        subject2.enableRootMotion = false;

        subjectAnimationInteract1.RestartSubject(subject1, anchorPos, anchorDir);
        subjectAnimationInteract2.RestartSubject(subject2, anchorPos, anchorDir);

        subject1.transform.position = this.subject1_Enter_Pos;
        subject1.transform.rotation = Quaternion.LookRotation(this.subject1_Enter_Dir);

        subject2.transform.position = this.subject2_Enter_Pos;
        subject2.transform.rotation = Quaternion.LookRotation(this.subject2_Enter_Dir);

        subject1.animator.CrossFade(
                "Default"
                , AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration
                , 0
                );

        subject2.animator.CrossFade(
            "Default"
            , AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration
            , 0
            );

        this.triggerRestart = false;
    }


    // Update is called once per frame
    void Update()
    {
        if(triggerRestart)
            this.Restart();

        this.subjectAnimationInteract1.UpdateInteract(Time.deltaTime);
        this.subjectAnimationInteract2.UpdateInteract(Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.subjectAnimationInteract1.exitPosition, .35f);
        Gizmos.DrawRay(this.subjectAnimationInteract1.exitPosition, this.subjectAnimationInteract1.exitRotation * Vector3.forward);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(this.subjectAnimationInteract2.exitPosition, .35f);
        Gizmos.DrawRay(this.subjectAnimationInteract2.exitPosition, this.subjectAnimationInteract2.exitRotation * Vector3.forward);
    }
}
