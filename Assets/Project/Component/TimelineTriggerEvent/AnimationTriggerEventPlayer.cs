using System;
using UnityEngine;

public class AnimationTriggerEventPlayer : TimelineTriggerEvent
{
    public float startTimer { get; private set; }
    public float endTimer { get; private set; }
    public AnimationClip animationClip { get; private set; }
    public float enterNormalizedTime { get; private set; }
    public float endNormalizedTime { get; private set; }

    public AnimationTriggerEventPlayer(AnimationTriggerEventSCRP animationTriggerEventSCRP)
        : this(animationTriggerEventSCRP.clip,
               animationTriggerEventSCRP.enterNormalizedTime,
               animationTriggerEventSCRP.endNormalizedTime,
               animationTriggerEventSCRP.triggerEventDetail)
    {
    }

    public AnimationTriggerEventPlayer(AnimationClip animationClip, float enterNormalized, float endNormalized,
        AnimationTriggerEventDetail[] triggerEventDetail)
        : base(animationClip.length, triggerEventDetail)
    {
        this.animationClip = animationClip;
        this.enterNormalizedTime = enterNormalized;
        this.endNormalizedTime = endNormalized;
        startTimer = animationClip.length * enterNormalized;
        endTimer = animationClip.length * endNormalized;
    }

    public override void Rewind() => RewindAt(startTimer);

    public override bool IsPlayFinish() => timer >= endTimer;

    // Kept for callers: EnemySpinKickGunFuNodeLeaf, AttackMoveMeleeWeaponNodeLeaf, QuickShootRangeWeaponNodeLeaf
    public bool GetNormalizedTimeFromStateName(string stateName, out float normalizedTime)
    {
        normalizedTime = 0;
        if (animationTriggerEventsDetails == null || animationTriggerEventsDetails.Length <= 0)
            return false;
        normalizedTime = GetEventNormalizedTime(stateName);
        return true;
    }
}
