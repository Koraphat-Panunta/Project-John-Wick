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
    public float[] triggerTimerEventNormalized;
    private Dictionary<string, int> getIndexAction { get; set; }
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
              , animationTriggerEventSCRP.triggerEventDetail)
    {
        
    }
    public AnimationTriggerEventPlayer(AnimationClip animationClip,float enterNormalized,float endNormalized, AnimationTriggerEventDetail[] triggerEventDetail)
    {
        this.animationClip = animationClip;
        this.enterNormalizedTime = enterNormalized;
        this.endNormalizedTime = endNormalized;
        startSelectCount = 0;
        startTimer = animationClip.length * enterNormalized;
        endTimer = animationClip.length * endNormalized;

        getIndexAction = new Dictionary<string, int>();
        getSelectEvent = new Dictionary<int, Action>();
        getEventTimer = new Dictionary<Action, float>();
        isAlreadyTrigger = new Dictionary<Action, bool>();

        this.triggerTimerEventNormalized = new float[triggerEventDetail.Length];

        List<float> allTriggerTimerNor = new List<float>();
        List<AnimationTriggerEventDetail> allAnimationTriggerEventDetails = new List<AnimationTriggerEventDetail>();
        for(int indexPopulate = 0;indexPopulate < triggerEventDetail.Length;indexPopulate++) 
        {
            allTriggerTimerNor.Add(triggerEventDetail[indexPopulate].normalizedTime);
            allAnimationTriggerEventDetails.Add(triggerEventDetail[indexPopulate]);
        }

        for (int i = 0; i < eventCount; i++)
        {
            float leastNumber = 1;
            int removeIndex = 0;
            for (int y = 0; y < allTriggerTimerNor.Count; y++)
            {
                if (leastNumber > allTriggerTimerNor[y])
                {
                    leastNumber = allTriggerTimerNor[y];
                    removeIndex = y;
                }

            }
            allTriggerTimerNor.RemoveAt(removeIndex);

            this.triggerTimerEventNormalized[i] = leastNumber;

            for (int z = 0; z < allAnimationTriggerEventDetails.Count; z++) 
            {
                if (allAnimationTriggerEventDetails[z].normalizedTime == leastNumber)
                {
                    this.getIndexAction.Add(allAnimationTriggerEventDetails[z].eventName, i);

                    allAnimationTriggerEventDetails.RemoveAt(z);

                    break;
                }
            }

            this.getSelectEvent.Add(i, new Action(() => { }));


            if (startTimer > animationClip.length
            * triggerEventDetail[i].normalizedTime)
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


        if (this.IsPlayFinish())
            return;

        timer += deltaTime;

        if (selectCount >= eventCount)
            return;

        if(getEventTimer.Count <= 0)
            return;

        while(selectCount < eventCount && timer >= this.animationClip.length * getEventTimer[getSelectEvent[selectCount]]
            && isAlreadyTrigger[getSelectEvent[selectCount]] == false)
        {
            getSelectEvent[selectCount].Invoke();
            isAlreadyTrigger[getSelectEvent[selectCount]] = true;
            selectCount++;

            //Debug.Log("selectCount " + selectCount);
            //Debug.Log("timer " + timer);
            //Debug.Log("this.animationClip.length * getEventTimer[getSelectEvent[selectCount] " + this.animationClip.length * getEventTimer[getSelectEvent[selectCount]]);
        }
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
        getSelectEvent[getIndexAction[eventName]] += subScribeEvent;
        isAlreadyTrigger.Add(getSelectEvent[getIndexAction[eventName]], false);
        getEventTimer.Add(getSelectEvent[getIndexAction[eventName]], this.triggerTimerEventNormalized[getIndexAction[eventName]]);
    }
}
