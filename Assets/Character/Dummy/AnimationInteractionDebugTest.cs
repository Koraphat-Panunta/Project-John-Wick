using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AnimationInteractionDebugTest : MonoBehaviour
{
    [SerializeField] private AnimationInteractScriptableObject animationInteractScriptableObject;

    private SubjectAnimationInteract subjectAnimationInteract1;
    private SubjectAnimationInteract subjectAnimationInteract2;

    [SerializeField] private Character subject1;
    [SerializeField] private string subject1Animation;
    [SerializeField] private Vector3 subject1_Enter_Pos;
    [SerializeField] private Vector3 subject1_Enter_Dir;

    [SerializeField] private Character subject2;
    [SerializeField] private string subject2Animation;
    [SerializeField] private Vector3 subject2_Enter_Pos;
    [SerializeField] private Vector3 subject2_Enter_Dir;

    [SerializeField] private Transform anchorTransform;
    [SerializeField] private Vector3 anchorPos ;
    [SerializeField] private Vector3 anchorDir;
    [SerializeField] private bool triggerRestart;

    private bool isEndTrigger;
    private void Awake()
    {
        this.subject1.Initialized();
        this.subject2.Initialized();

        this.subject1_Enter_Pos = this.subject1.transform.position;
        this.subject1_Enter_Dir = this.subject1.transform.forward;

        this.subject2_Enter_Pos = this.subject2.transform.position;
        this.subject2_Enter_Dir = this.subject2.transform.forward;


        subjectAnimationInteract1 = new SubjectAnimationInteract(
            animationInteractScriptableObject.clip
            , animationInteractScriptableObject.enterNormalizedTime
            , animationInteractScriptableObject.endNormalizedTime
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
        if (character == subject1)
        {
            subject1.animator.CrossFade(
                subject1Animation
                , 0
                , 0
                , subjectAnimationInteract1.animationInteractCharacterDetail.enterAnimationOffsetNormalizedTime);
        }

        if (character == subject2)
        {
            
            subject2.animator.CrossFade(
                subject2Animation
                , 0
                , 0
                , subjectAnimationInteract2.animationInteractCharacterDetail.enterAnimationOffsetNormalizedTime);
        }
    }
    private bool isInteract;
    int debug;
    private async Task DelayRootMotion(Character character)
    {
        await Task.Yield();
        await Task.Yield();
        character.enableRootMotion = true;
    }
    private void BeginInteract(Character character)
    {
        _ = SubjectAnimationInteract.DelayRootMotion(character);

        //character.enableRootMotion = true;
        //if(character == subjectAnimationInteract2.character)
        //{
        //    isInteract = true;
        //}

        //if(character == subjectAnimationInteract1.character)
        //{
        //    Debug.Log("Character : " + character + " anchor Distance pos = "
        //       + Vector3.Distance(
        //           character.transform.position
        //           , subjectAnimationInteract1.anhorPosition)
        //       );
        //    Debug.Log("Character : " + character +" anchor Distance rot = "
        //    + Quaternion.Angle(
        //        character.transform.rotation
        //        , Quaternion.LookRotation(subjectAnimationInteract1.anhorDir))
        //    );
        //}

        //if (character == subjectAnimationInteract2.character)
        //{
        //    Debug.Log("Character : " + character + " anchor Distance pos = "
        //       + Vector3.Distance(
        //           character.transform.position
        //           , subjectAnimationInteract2.anhorPosition)
        //       );
        //    Debug.Log("Character : " + character + " anchor Distance rot = "
        //    + Quaternion.Angle(
        //        character.transform.rotation
        //        , Quaternion.LookRotation(subjectAnimationInteract2.anhorDir))
        //    );
        //}

    }

    public void Restart()
    {
        this.isEndTrigger = false;

        subject1.enableRootMotion = false;
        subject2.enableRootMotion = false;

        subject1.animator.CrossFade("Rest", 0, 0, 0);
        subject2.animator.CrossFade("Dodge", 0, 0, 0);

        subject1._movementCompoent.CancleMomentum();
        subject2._movementCompoent.CancleMomentum();

        subject1._movementCompoent.SetPosition(this.subject1_Enter_Pos);
        subject1._movementCompoent.SetRotation(Quaternion.LookRotation(this.subject1_Enter_Dir));

        subject2._movementCompoent.SetPosition(this.subject2_Enter_Pos);
        subject2._movementCompoent.SetRotation(Quaternion.LookRotation(this.subject2_Enter_Dir));

                
        subjectAnimationInteract1.RestartSubject(subject1, anchorPos, anchorDir);
        subjectAnimationInteract2.RestartSubject(subject2, anchorPos, anchorDir);

        this.triggerRestart = false;

        isInteract = false;
        debug = 0;
    }


    // Update is called once per frame
    void Update()
    {
        //if (isInteract)
        //    debug += 1;

        //if(debug == 2)
        //{
        //    Debug.Log("");
        //    subjectAnimationInteract1.character.enableRootMotion = true;
        //    subjectAnimationInteract2.character.enableRootMotion = true;
        //}

        if(triggerRestart)
            this.Restart();

        this.subjectAnimationInteract1.UpdateInteract(Time.deltaTime);
        this.subjectAnimationInteract2.UpdateInteract(Time.deltaTime);

        if (this.subjectAnimationInteract1.animationTriggerEventPlayer.IsPlayFinish())
            subject1.enableRootMotion = false;
        
        if (this.subjectAnimationInteract2.animationTriggerEventPlayer.IsPlayFinish())
            subject2.enableRootMotion = false;
        
        if(this.subjectAnimationInteract1.animationTriggerEventPlayer.IsPlayFinish()
            && isEndTrigger == false)
        {
            isEndTrigger = true;
            this.OnEndEvent();
        }
    }
   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(anchorPos, .15f);
        Gizmos.DrawRay(anchorPos, anchorDir);

        this.CalculateAdjustTransform(this.animationInteractScriptableObject.animationInteractCharacterDetail[0], out Vector3 finalPosSub1, out Vector3 finalDirSub1);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(finalPosSub1, .15f);
        Gizmos.DrawRay(finalPosSub1, finalDirSub1);

        this.CalculateAdjustTransform(this.animationInteractScriptableObject.animationInteractCharacterDetail[1], out Vector3 finalPosSub2, out Vector3 finalDirSub2);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(finalPosSub2, .15f);
        Gizmos.DrawRay(finalPosSub2, finalDirSub2);
    }

    public void CalculateAdjustTransform(AnimationInteractCharacterDetail animationInteractCharacterDetail,out Vector3 finalPos,out Vector3 finalDir)
    {
        finalPos = this.anchorPos
            + (this.anchorDir.normalized * animationInteractCharacterDetail.offsetPositionFormAnchor.y)
            + (Vector3.Cross(Vector3.up, this.anchorDir.normalized).normalized * animationInteractCharacterDetail.offsetPositionFormAnchor.x);

        Quaternion sub1ExitRot = Quaternion.LookRotation(this.anchorDir.normalized, Vector3.up) * Quaternion.Euler(0, animationInteractCharacterDetail.RotationRelative, 0);
        finalDir = sub1ExitRot * Vector3.forward;
    }

    private void OnEndEvent()
    {
        Debug.Log("Character : " + subjectAnimationInteract1.character + " final anchor Distance pos = "
        + Vector3.Distance(
                   subjectAnimationInteract1.character.transform.position
                  , subjectAnimationInteract1.anhorPosition)
              );
        Debug.Log("Character : " + subjectAnimationInteract1.character + " final anchor Distance rot = "
        + Quaternion.Angle(
             subjectAnimationInteract1.character.transform.rotation
            , Quaternion.LookRotation(subjectAnimationInteract1.anhorDir))
        );

        Debug.Log("Character : " + subjectAnimationInteract2.character + " final anchor Distance pos = "
        + Vector3.Distance(
                   subjectAnimationInteract2.character.transform.position
                  , subjectAnimationInteract2.anhorPosition)
              );
        Debug.Log("Character : " + subjectAnimationInteract2.character + " final anchor Distance rot = "
        + Quaternion.Angle(
             subjectAnimationInteract2.character.transform.rotation
            , Quaternion.LookRotation(subjectAnimationInteract2.anhorDir))
        );
    }
  
}
