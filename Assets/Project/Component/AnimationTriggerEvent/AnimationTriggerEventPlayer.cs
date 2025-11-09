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
    public float[] triggerTimerEventNormalized;
    private Dictionary<int, Action> getSelectEvent { get; set; }
    private Dictionary<Action,float> getEventTimer { get; set; }
    private Dictionary<Action, bool> isAlreadyTrigger { get; set; }
    public AnimationClip animationClip { get; protected set; }

    private int eventCount => triggerTimerEventNormalized.Length;
    private int startSelectCount;
    private int selectCount;

    public float enterNormalizedTime;
    public float endNormalizedTime;

    //private AnimationTriggerEventSCRP animationTriggerEventSCRP;
    public AnimationTriggerEventPlayer(AnimationTriggerEventSCRP animationTriggerEventSCRP) 
        : this(animationTriggerEventSCRP.clip
              , animationTriggerEventSCRP.enterNormalizedTime
              , animationTriggerEventSCRP.endNormalizedTime
              , animationTriggerEventSCRP.triggerTimerEventNormalized)
    {
        
    }
    public AnimationTriggerEventPlayer(AnimationClip animationClip,float enterNormalized,float endNormalized, float[] triggerTimerEventNormalized)
    {
        this.animationClip = animationClip;
        this.enterNormalizedTime = enterNormalized;
        this.endNormalizedTime = endNormalized;
        startSelectCount = 0;
        startTimer = animationClip.length * enterNormalized;
        endTimer = animationClip.length * endNormalized;

        getSelectEvent = new Dictionary<int, Action>();
        getEventTimer = new Dictionary<Action, float>();
        isAlreadyTrigger = new Dictionary<Action, bool>();

        this.triggerTimerEventNormalized = new float[triggerTimerEventNormalized.Length];

        for (int i = 0; i < eventCount; i++)
        {
            this.triggerTimerEventNormalized[i] = triggerTimerEventNormalized[i];
            this.getSelectEvent.Add(i, new Action(() => { }));
            if (startTimer > animationClip.length
            * triggerTimerEventNormalized[i])
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

        if (timer >= this.animationClip.length * getEventTimer[getSelectEvent[selectCount]]
            && isAlreadyTrigger[getSelectEvent[selectCount]] == false)
        {

            Debug.Log("selectCount " + selectCount);
            Debug.Log("timer " + timer);
            Debug.Log("this.animationClip.length * getEventTimer[getSelectEvent[selectCount] " + this.animationClip.length * getEventTimer[getSelectEvent[selectCount]]);

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
        getSelectEvent[i] += subScribeEvent;
        isAlreadyTrigger.Add(getSelectEvent[i], false);
        getEventTimer.Add(getSelectEvent[i], this.triggerTimerEventNormalized[i]);
    }
}
