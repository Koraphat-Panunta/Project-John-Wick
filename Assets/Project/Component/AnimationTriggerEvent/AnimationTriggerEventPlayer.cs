using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AnimationTriggerEventPlayer 
{
    public float startTimer { get; protected set; }
    public float timer { get; protected set; }
    public float timerNormalized { get => timer/endTimer; }
    public float endTimer { get; protected set; }
    private AnimationTriggerEventDetail[] animationTriggerEventsDetails;
    private Dictionary<AnimationTriggerEventDetail, bool> isAlreadyTrigger;
    private Dictionary<AnimationTriggerEventDetail, Action> animationTriggerEventAction;
    public AnimationClip animationClip { get; protected set; }

    private int eventCount => animationTriggerEventsDetails.Length;

    public float enterNormalizedTime;
    public float endNormalizedTime;

    //private AnimationTriggerEventSCRP animationTriggerEventSCRP;
    public AnimationTriggerEventPlayer(AnimationTriggerEventSCRP animationTriggerEventSCRP) 
        : this(animationTriggerEventSCRP.clip
              , animationTriggerEventSCRP.enterNormalizedTime
              , animationTriggerEventSCRP.endNormalizedTime
              , animationTriggerEventSCRP.triggerEventDetail)
    {
        
    }
    public AnimationTriggerEventPlayer(AnimationClip animationClip,float enterNormalized,float endNormalized, AnimationTriggerEventDetail[] triggerEventDetail)
    {
        this.animationClip = animationClip;
        this.enterNormalizedTime = enterNormalized;
        this.endNormalizedTime = endNormalized;

        startTimer = animationClip.length * enterNormalized;
        endTimer = animationClip.length * endNormalized;

        this.PopulateProperties(triggerEventDetail);

    }

    private void PopulateProperties(AnimationTriggerEventDetail[] triggerEventDetail)
    {
        if (triggerEventDetail == null || triggerEventDetail.Length <= 0)
            return;

        animationTriggerEventsDetails = new AnimationTriggerEventDetail[triggerEventDetail.Length];
        isAlreadyTrigger = new Dictionary<AnimationTriggerEventDetail, bool>();
        animationTriggerEventAction = new Dictionary<AnimationTriggerEventDetail, Action>();


        for (int i = 0; i < triggerEventDetail.Length; i++)
        {
            animationTriggerEventsDetails[i] = triggerEventDetail[i];
            isAlreadyTrigger.Add(animationTriggerEventsDetails[i], true);
            animationTriggerEventAction.Add(animationTriggerEventsDetails[i], new Action(() => { }));
        }
    }
    private void RewindPopulateProperties()
    {
        if(animationTriggerEventsDetails == null || animationTriggerEventsDetails.Length <= 0)
            return;

        for (int i = 0; i < animationTriggerEventsDetails.Length; i++)
        {
            if (startTimer < animationTriggerEventsDetails[i].normalizedTime * animationClip.length)
            {
                isAlreadyTrigger[animationTriggerEventsDetails[i]] = false;
            }
        }
    }
    private void UpdateProperties()
    {
        if(animationTriggerEventsDetails == null || animationTriggerEventsDetails.Length <= 0)
            return;

        for (int i = 0; i < animationTriggerEventsDetails.Length; i++)
        {
            if (isAlreadyTrigger[animationTriggerEventsDetails[i]])
                continue;

            if (timer >= animationTriggerEventsDetails[i].normalizedTime * animationClip.length)
            {
                animationTriggerEventAction[animationTriggerEventsDetails[i]].Invoke();
                isAlreadyTrigger[animationTriggerEventsDetails[i]] = true;
            }
        }
    }
    public void Rewind()
    {
        timer = startTimer;
        this.RewindPopulateProperties();
       
    }

    public void UpdatePlay(float deltaTime)
    {


        if (this.IsPlayFinish())
            return;

        this.UpdateProperties();

        timer += deltaTime;

    }

    public bool IsPlayFinish()
    {
        return timer >= endTimer;
    }

    public float GetRemapNormalizedTimer(float enterNormalized,float exitNormalized)
    {
        float normal = 0;

        normal = (timer - (animationClip.length * enterNormalized)) /  ((animationClip.length * exitNormalized) - (animationClip.length * enterNormalized));

        return normal;
    }

    public void SubscribeEvent(string eventName,Action subScribeEvent)
    {
        for (int i = 0; i < animationTriggerEventsDetails.Length; i++) 
        {
            if (animationTriggerEventsDetails[i].eventName == eventName)
            {
                animationTriggerEventAction[animationTriggerEventsDetails[i]] += subScribeEvent;
            }
        }
    }
}
