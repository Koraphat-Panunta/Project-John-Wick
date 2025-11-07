using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationTriggerEventPlayer 
{
    public float startTimer { get; protected set; }
    public float timer { get; protected set; }
    public float timerNormalized { get => timer/endTimer; }
    public float endTimer { get; protected set; }
    private Dictionary<int, Action> getSelectEvent { get; set; }
    private Dictionary<Action,float> getEventTimer { get; set; }
    private Dictionary<Action, bool> isAlreadyTrigger { get; set; }
    public AnimationTriggerEventSCRP animationTriggerEventSCRP { get; protected set; }
    private int eventCount => animationTriggerEventSCRP.triggerTimerEventNormalized.Length;
    private int startSelectCount;
    private int selectCount;
    public AnimationTriggerEventPlayer(AnimationTriggerEventSCRP animationTriggerEventSCRP)
    {
        this.animationTriggerEventSCRP = animationTriggerEventSCRP;
        startSelectCount = 0;
        startTimer = animationTriggerEventSCRP.clip.length * animationTriggerEventSCRP.enterNormalizedTime;
        endTimer = animationTriggerEventSCRP.clip.length * animationTriggerEventSCRP.endNormalizedTime;

        getSelectEvent = new Dictionary<int, Action>();
        getEventTimer = new Dictionary<Action, float>();
        isAlreadyTrigger = new Dictionary<Action, bool>();

        for (int i = 0; i < eventCount; i++)
        {
            if (startTimer > animationTriggerEventSCRP.clip.length
            * animationTriggerEventSCRP.triggerTimerEventNormalized[i])
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
            isAlreadyTrigger[getSelectEvent[i]] = false;
        }
    }

    public void UpdatePlay(float deltaTime)
    {
        if(this.IsPlayFinish())
            return;

        timer += deltaTime;

        if(selectCount >= eventCount)
            return;
        
        if(timer >= animationTriggerEventSCRP.clip.length * getEventTimer[getSelectEvent[selectCount]]
            && isAlreadyTrigger[getSelectEvent[selectCount]] == false)
        {
            getSelectEvent[selectCount].Invoke();
            isAlreadyTrigger[getSelectEvent[selectCount]] = true;
            selectCount++;
        }
    }

    public bool IsPlayFinish()
    {
        return timer >= endTimer;
    }

    public void SubscribeEvent(int i,Action subScribeEvent)
    {
        getSelectEvent.Add(i, subScribeEvent);
        isAlreadyTrigger.Add(getSelectEvent[i], false);
        getEventTimer.Add(getSelectEvent[i], animationTriggerEventSCRP.triggerTimerEventNormalized[i]);
    }
}
