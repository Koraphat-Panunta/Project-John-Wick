using System.Collections.Generic;
using System;
using UnityEngine;

public class TimelineTriggerEvent 
{

    public float timer { get; protected set; }
    public float timerNormalized { get => timer / timeDuration; }
    public virtual float timeDuration { get; protected set; }
    private AnimationTriggerEventDetail[] animationTriggerEventsDetails;
    private Dictionary<AnimationTriggerEventDetail, bool> isAlreadyTrigger;
    private Dictionary<AnimationTriggerEventDetail, Action> animationTriggerEventAction;


    private int eventCount => animationTriggerEventsDetails.Length;

    


    //private AnimationTriggerEventSCRP animationTriggerEventSCRP;
    public TimelineTriggerEvent(TimelineTriggerEventScriptableObject timelineTriggerEventScriptableObject)
        :this(
             timelineTriggerEventScriptableObject.timeDuration
             ,timelineTriggerEventScriptableObject.triggerEventDetail
             )
    {

    }
    public TimelineTriggerEvent(float timeDuration, AnimationTriggerEventDetail[] triggerEventDetail)
    {

        this.timeDuration = timeDuration;

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
        if (animationTriggerEventsDetails == null || animationTriggerEventsDetails.Length <= 0)
            return;

        for (int i = 0; i < animationTriggerEventsDetails.Length; i++)
        {

            isAlreadyTrigger[animationTriggerEventsDetails[i]] = false;
        }
    }
    private void UpdateProperties()
    {
        if (animationTriggerEventsDetails == null || animationTriggerEventsDetails.Length <= 0)
            return;

        for (int i = 0; i < animationTriggerEventsDetails.Length; i++)
        {
            if (isAlreadyTrigger[animationTriggerEventsDetails[i]])
                continue;

            if (timer >= animationTriggerEventsDetails[i].normalizedTime * timeDuration)
            {
                animationTriggerEventAction[animationTriggerEventsDetails[i]].Invoke();
                isAlreadyTrigger[animationTriggerEventsDetails[i]] = true;
            }
        }
    }
    public void Rewind()
    {
        this.RewindAt(0);
    }
    public void RewindAt(float time)
    {
        this.timer = Mathf.Clamp(time,0,this.timeDuration);
        this.RewindPopulateProperties();
    }

    public void UpdatePlay(float deltaTime)
    {


        if (this.IsPlayFinish())
            return;

        this.UpdateProperties();

        timer += deltaTime;

    }

    public bool IsPlayFinish(float endNormalized)
    {
        return timer >= timeDuration * endNormalized;
    }
    public bool IsPlayFinish()
    {
        return this.IsPlayFinish(1);
    }

    public float GetRemapNormalizedTimer(float enterNormalized, float exitNormalized)
    {
        float normal = 0;

        normal = (timer - (this.timeDuration * enterNormalized)) / ((this.timeDuration * exitNormalized) - (this.timeDuration * enterNormalized));

        return normal;
    }

    public void SubscribeEvent(string eventName, Action subScribeEvent)
    {
        bool isFoundTheName = false;

        for (int i = 0; i < animationTriggerEventsDetails.Length; i++)
        {
            //Debug.Log("animationTriggerEventsDetails[i].eventName = "+ animationTriggerEventsDetails[i].eventName);
            if (animationTriggerEventsDetails[i].eventName == eventName)
            {
                isFoundTheName = true;
                animationTriggerEventAction[animationTriggerEventsDetails[i]] += subScribeEvent;
            }
        }

        if (isFoundTheName == false)
            Debug.LogError("Not found the name event = " + eventName);
    }
    public float GetEventNormalizedTime(string eventName)
    {

        bool isFoundTheName = false;

        for (int i = 0; i < animationTriggerEventsDetails.Length; i++)
        {
            //Debug.Log("animationTriggerEventsDetails[i].eventName = "+ animationTriggerEventsDetails[i].eventName);
            if (animationTriggerEventsDetails[i].eventName == eventName)
            {
                isFoundTheName = true;
                return animationTriggerEventsDetails[i].normalizedTime;
            }
        }

        if (isFoundTheName == false)
            Debug.LogError("Not found the name event = " + eventName);

        return 0f;
    }
}
