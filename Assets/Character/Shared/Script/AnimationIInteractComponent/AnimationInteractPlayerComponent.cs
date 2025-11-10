using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System;

public class AnimationInteractPlayerComponent 
{

    public AnimationClip animationClip;
    public float playOffsetNormalized;
    public float endOfsetNormalized;
    private SubjectAnimationMotionWarping[] subjectAnimationMotionWarpings;
    private Dictionary<string, SubjectAnimationMotionWarping> subjectNameToSubjectInteract;

    public Vector3 anchorPosition { get; protected set; }
    public Vector3 anchorDir { get; protected set; }

    public enum WarpingEvent
    {
        BeginWarping,
        FinishWarping,
        BeginPlayAnimation
    }
    public AnimationInteractPlayerComponent(
        AnimationClip animationClip
        ,float playOffsetNormalized
        ,float endOfsetNormalized
        , AnimationInteractCharacterDetail[] animationInteractCharacterDetails)
    {
        this.animationClip = animationClip;
        this.playOffsetNormalized = playOffsetNormalized;
        this.endOfsetNormalized = endOfsetNormalized;
    }
    public void AssignCharacter(Character character,string subjectName)
    {
        this.subjectNameToSubjectInteract[subjectName].character = character;
    }
    public void SetAnchor(Vector3 anchorPosition,Vector3 anchorDir)
    {
        this.anchorPosition = anchorPosition;
        this.anchorDir = anchorDir;
    }
    public void BeginInteract()
    {
        for (int x = 0; x < this.subjectAnimationMotionWarpings.Length; x++) 
        {
            this.subjectAnimationMotionWarpings[x].Reset();
        }
    }
    public void UpdateInteract(float deltaTime)
    {
       
    }
    
    
    public class SubjectAnimationMotionWarping
    {
        public AnimationClip animationClip;

        public AnimationInteractCharacterDetail animationInteractCharacterDetail;

        public Character character;

        public AnimationTriggerEventPlayer animationTriggerEventPlayer;

        public string subjectName => animationInteractCharacterDetail.subjectName;

        public Vector3 anhorPosition { get; protected set; }
        public Quaternion anhorRotation { get; protected set; }

        public Vector3 enterPosition { get; protected set; }
        public Quaternion enterRotation { get; protected set; }

        public Vector3 exitPosition { get; protected set; }
        public Quaternion exitRotation { get; protected set; }

        public bool isWarping { get; protected set; }

        public Action<Character> beginWarpEvent = new Action<Character>((character) => { });
        public Action<Character> finishWarpEvent = new Action<Character>((character) => { });
        public Action<Character> beginPlayAnimationEvent = new Action<Character>((character) => { });

        public void Reset()
        {
            enterPosition = Vector3.zero;
            exitPosition = Vector3.zero;

            enterRotation = Quaternion.identity;
            exitRotation = Quaternion.identity;

            isWarping = false;
        }

        public void UpdateInteract(float deltaTime)
        {
            if (isWarping)
            {

            }
        }

        public void BeginWarp(Vector3 anchorPosition,Vector3 anchorDirection)
        {
            isWarping = true;
            this.CalculateAdjustTransform(anchorPosition, anchorDirection);
            this.beginWarpEvent.Invoke(this.character);
        }
        public void CalculateAdjustTransform(Vector3 anchorPosition, Vector3 anchorDirection)
        {
            enterPosition = this.character.transform.position;
            enterRotation = this.character.transform.rotation;

            exitPosition = anchorPosition
                + (anchorDirection.normalized * animationInteractCharacterDetail.offsetPositionFormAnchor.y)
                + (Vector3.Cross(Vector3.up, anchorDirection.normalized).normalized * animationInteractCharacterDetail.offsetPositionFormAnchor.x);
            exitRotation = Quaternion.LookRotation(anchorDirection, Vector3.up) * Quaternion.Euler(0, animationInteractCharacterDetail.RotationRelative, 0);
        }
        public void StopWarpBeginRootEnable()
        {
            isWarping = false;
            this.finishWarpEvent.Invoke(this.character);
        }
        public void BeginPlayAnimation()
        {
            this.beginPlayAnimationEvent.Invoke(this.character);
        }
     
    }

}
