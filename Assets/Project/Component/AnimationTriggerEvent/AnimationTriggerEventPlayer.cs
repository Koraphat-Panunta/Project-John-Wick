using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationTriggerEventPlayer 
{
    private float startTimer;
    public float timer { get; protected set; }
    public float timerNormalized { get => timer/endTimer; }
    private float endTimer;
    private Dictionary<EventTriggerAnimation, bool> isAlreadyTrigger { get; set; }
    public AnimationTriggerEventSCRP animationTriggerEventSCRP { get; protected set; }
    private int eventCount => animationTriggerEventSCRP.eventTriggerAnimations.Count;
    private int startSelectCount;
    private int selectCount;
    public AnimationTriggerEventPlayer(AnimationTriggerEventSCRP animationTriggerEventSCRP)
    {
        this.animationTriggerEventSCRP = animationTriggerEventSCRP;
        startSelectCount = 0;
        startTimer = animationTriggerEventSCRP.clip.length * animationTriggerEventSCRP.enterNormalizedTime;
        endTimer = animationTriggerEventSCRP.clip.length * animationTriggerEventSCRP.endNormalizedTime;

        isAlreadyTrigger = new Dictionary<EventTriggerAnimation, bool>();

        for (int i = 0; i < eventCount; i++)
        {
            isAlreadyTrigger.Add(animationTriggerEventSCRP.eventTriggerAnimations[i],false);

            if (timer > animationTriggerEventSCRP.clip.length
            * animationTriggerEventSCRP.eventTriggerAnimations[i].timerNormalized)
            {
                startSelectCount++;
            }
        }
    }

    public void Rewind()
    {
        timer = startTimer;
        selectCount = startSelectCount;
        for (int i = 0; i < eventCount; i++)
        {
            isAlreadyTrigger[animationTriggerEventSCRP.eventTriggerAnimations[i]] = false;
        }
    }

    public void UpdatePlay(float deltaTime)
    {
        if(this.IsPlayFinish())
            return;

        timer += deltaTime;
        
        if( (selectCount < animationTriggerEventSCRP.eventTriggerAnimations.Count)
            && (timer >= 
            animationTriggerEventSCRP.clip.length 
            * animationTriggerEventSCRP.eventTriggerAnimations[selectCount].timerNormalized)
            && (isAlreadyTrigger[animationTriggerEventSCRP.eventTriggerAnimations[startSelectCount]] == false)
            )
        {
            animationTriggerEventSCRP.eventTriggerAnimations[selectCount].unityEvent.Invoke();
            selectCount++;
        }
    }

    public bool IsPlayFinish()
    {
        return timer >= endTimer;
    }

    public void SubscribeEvent(int i,UnityAction subScribeEvent)
    {
        try
        {
            animationTriggerEventSCRP.eventTriggerAnimations[i].unityEvent.AddListener(subScribeEvent);
        }
        catch
        {
            throw new Exception("subscribe event failed "+subScribeEvent + " on event number "+i);
        }
    }
}
