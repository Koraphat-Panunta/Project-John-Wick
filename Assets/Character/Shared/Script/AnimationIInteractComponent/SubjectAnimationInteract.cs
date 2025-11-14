using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System;

public class SubjectAnimationInteract 
{

    public AnimationClip animationClip;

    public AnimationInteractCharacterDetail animationInteractCharacterDetail;

    protected Character character;

    public AnimationTriggerEventPlayer animationTriggerEventPlayer;

    public string subjectName => animationInteractCharacterDetail.subjectName;

    public Vector3 anhorPosition { get; protected set; }
    public Vector3 anhorDir { get; protected set; }

    public Vector3 enterPosition { get; protected set; }
    public Quaternion enterRotation { get; protected set; }

    public Vector3 exitPosition { get; protected set; }
    public Quaternion exitRotation { get; protected set; }

    public bool isWarping { get; protected set; }

    public Action<Character> beginWarpEvent = new Action<Character>((character) => { });
    public Action<Character> finishWarpEvent = new Action<Character>((character) => { });
    public Action<Character> beginPlayAnimationEvent = new Action<Character>((character) => { });

    public SubjectAnimationInteract(AnimationClip animationClip,float enterNormalized,float finishNormalized,AnimationInteractCharacterDetail animationInteractCharacterDetail)
    {
        this.animationInteractCharacterDetail = animationInteractCharacterDetail;
        AnimationTriggerEventDetail[] animationTriggerEventDetail = new AnimationTriggerEventDetail[3];
        animationTriggerEventDetail[0] = new AnimationTriggerEventDetail
        {
            eventName = "BeginWarp",
            normalizedTime = animationInteractCharacterDetail.beginWarpingNormalizedTime_BasedOnMainSubject
        };
        animationTriggerEventDetail[1] = new AnimationTriggerEventDetail
        {
            eventName = "FinishWarp",
            normalizedTime = animationInteractCharacterDetail.finishWarpingNormalizedTime_BasedOnMainSubject
        };
        animationTriggerEventDetail[2] = new AnimationTriggerEventDetail
        {
            eventName = "BeginPlayAnimation",
            normalizedTime = animationInteractCharacterDetail.beginPlayAnimationNormalizedTime_BasedOnMainSubject
        };
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(animationClip, enterNormalized, finishNormalized, animationTriggerEventDetail);
        this.animationTriggerEventPlayer.SubscribeEvent("BeginWarp", BeginWarp);
        this.animationTriggerEventPlayer.SubscribeEvent("FinishWarp",StopWarpBeginRootEnable);
        this.animationTriggerEventPlayer.SubscribeEvent("BeginPlayAnimation",BeginPlayAnimation);

       
    }

    public void RestartSubject(Character character,Vector3 anchorPos,Vector3 anhorDir)
    {
        this.character = character;
        this.character._movementCompoent.CancleMomentum();
        this.anhorPosition = anchorPos;
        this.anhorDir = anhorDir;

        this.animationTriggerEventPlayer.Rewind();

        isWarping = false;
    }

    public virtual void UpdateInteract(float deltaTime)
    {

        if (isWarping)
        {
            float t = animationTriggerEventPlayer.GetRemapNormalizedTimer
                (animationInteractCharacterDetail.beginWarpingNormalizedTime_BasedOnMainSubject
                ,animationInteractCharacterDetail.finishWarpingNormalizedTime_BasedOnMainSubject);
            //Debug.Log("t = " + t);
            //Debug.Log("begin = " + animationInteractCharacterDetail.beginWarpingNormalizedTime_BasedOnMainSubject);
            //Debug.Log("exit = " + animationInteractCharacterDetail.finishWarpingNormalizedTime_BasedOnMainSubject);

            MovementWarper.WarpMovement(enterPosition, enterRotation, character._movementCompoent, exitPosition, exitRotation, t);
        }
        animationTriggerEventPlayer.UpdatePlay(deltaTime);
    }

    
    public void CalculateAdjustTransform()
    {
        enterPosition = this.character.transform.position;
        enterRotation = this.character.transform.rotation;

        exitPosition = this.anhorPosition
            + (this.anhorDir.normalized * animationInteractCharacterDetail.offsetPositionFormAnchor.y)
            + (Vector3.Cross(Vector3.up, this.anhorDir.normalized).normalized * animationInteractCharacterDetail.offsetPositionFormAnchor.x);
        exitRotation = Quaternion.LookRotation(this.anhorDir.normalized, Vector3.up) * Quaternion.Euler(0, animationInteractCharacterDetail.RotationRelative, 0);

    }
    private void BeginWarp()
    {
        Debug.Log("Subject " + character + " begin warp" + " timeNormalized = " + animationTriggerEventPlayer.timerNormalized);
        isWarping = true;
        this.CalculateAdjustTransform();
        this.beginWarpEvent.Invoke(this.character);
    }
    private void StopWarpBeginRootEnable()
    {
        Debug.Log("Subject " + character + " stop warp" + " timeNormalized = " + animationTriggerEventPlayer.timerNormalized);
        isWarping = false;
        this.finishWarpEvent.Invoke(this.character);
    }
    private void BeginPlayAnimation()
    {
        Debug.Log("Subject " + character + " begin play animation" + " timeNormalized = " + animationTriggerEventPlayer.timerNormalized);
        this.beginPlayAnimationEvent.Invoke(this.character);
    }

}
