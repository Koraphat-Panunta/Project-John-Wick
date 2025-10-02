using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DynamicPostProcessing : MonoBehaviour,IObserverPlayer,IObserverTimeControlManager,IInitializedAble
{
    [SerializeField] protected Player player;
    [SerializeField] protected TimeControlManager timeControlManager;
    [SerializeField] protected Volume volume;

    [SerializeField] protected Vignette vignette;
    [SerializeField] private float originVignetteIntensity;
    [SerializeField] private Color originVignetteColor;
    [SerializeField] private float originVignetteSmooth;

    [SerializeField] private float hurtVignetteIntensity;
    [SerializeField] private Color hurtVignetteColor;
    [SerializeField] private float hurtVignetteSmooth;

    private bool isBulletTime;
    private bool isPlayerRestrict_HumanShield;
    [SerializeField] protected ChromaticAberration chromaticAberration;
    [SerializeField] private float originChromaticAberration;
    [SerializeField] private float bulletTimeChromaticAberration;

    public void Initialized()
    {
        this.player.AddObserver(this);
        this.timeControlManager.AddObserver(this);

        if (this.volume != null)
        {
            this.volume.profile.TryGet<Vignette>(out this.vignette);
            this.volume.profile.TryGet<ChromaticAberration>(out this.chromaticAberration);
        }
    }
    private void Update()
    {
        this.HurtEffectCheck();
    }
    private void OnValidate()
    {
        this.player = FindAnyObjectByType<Player>();
        this.timeControlManager = FindAnyObjectByType<TimeControlManager>();

       
    }
    public void OnNotify<T>(Player player, T node)
    {
        if(node is HumanShield_GunFuInteraction_NodeLeaf humanShield_GunFuInteraction_Node
            && humanShield_GunFuInteraction_Node.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Enter
            || node is RestrictGunFuStateNodeLeaf restrictGunFuState_NodeLeaf
            && restrictGunFuState_NodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Enter)
        {
            isPlayerRestrict_HumanShield = true;
        }
        else if(node is HumanShield_GunFuInteraction_NodeLeaf humanShield_GunFuInteraction_NodeExit
            && humanShield_GunFuInteraction_NodeExit.curPhase == PlayerStateNodeLeaf.NodePhase.Exit
            || node is RestrictGunFuStateNodeLeaf restrictGunFuState_NodeLeafExit
            && restrictGunFuState_NodeLeafExit.curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
        {
            isPlayerRestrict_HumanShield = false;
        }

        this.UpdatePostProcessing();
    }

    public void OnNotifyObserver<T>(TimeControlManager timeControlManager, T Var)
    {
        if(Var is TriggerTimeSlowCurveNodeLeaf triggerTimeSlowCurveNodeLeaf
            && triggerTimeSlowCurveNodeLeaf.curPhase == TimeNodeLeaf.TimeNodeLeafPhase.Enter)
        {
            isBulletTime = true;    
        }
        else if(Var is TriggerTimeSlowCurveNodeLeaf triggerTimeSlowCurveNodeLeafExit
            && triggerTimeSlowCurveNodeLeafExit.curPhase == TimeNodeLeaf.TimeNodeLeafPhase.Exit)
            isBulletTime = false;

        this.UpdatePostProcessing();
    }

    private void UpdatePostProcessing()
    {
        if(isPlayerRestrict_HumanShield && isBulletTime)
        {
            if(bulletTime != null)
            {
                StopCoroutine(bulletTime);
                bulletTime = null;
            }
            bulletTime = StartCoroutine(BulletTimeEffect());
        }
    }

    private Coroutine bulletTime;
    private IEnumerator BulletTimeEffect()
    {
        while(isPlayerRestrict_HumanShield && isBulletTime)
        {
            chromaticAberration.intensity.value = Mathf.Clamp(chromaticAberration.intensity.value + Time.unscaledDeltaTime, originChromaticAberration, bulletTimeChromaticAberration);
            yield return null;
        }
        while(chromaticAberration.intensity.value > originChromaticAberration)
        {
            chromaticAberration.intensity.value = Mathf.Clamp(chromaticAberration.intensity.value - Time.unscaledDeltaTime, originChromaticAberration, bulletTimeChromaticAberration);
            yield return null;
        }
        bulletTime = null;
    }

    float halfMaxHP => player.GetMaxHp() * 0.5f;
    [SerializeField] float i = 0;
    private float ChnageRate = 0.25f;
    private void HurtEffectCheck() 
    {
       
        if (player.GetHP() < halfMaxHP)
        {
            float targetI = Mathf.Clamp01(player.GetHP() / halfMaxHP);

            i = Mathf.MoveTowards(i, 1 - targetI, Time.unscaledDeltaTime * this.ChnageRate);

            vignette.color = new ColorParameter(Color.Lerp(this.originVignetteColor, this.hurtVignetteColor, i));
            vignette.smoothness.value = Mathf.Clamp(i,originVignetteSmooth,hurtVignetteSmooth);
            vignette.intensity.value = Mathf.Clamp(i, originVignetteIntensity,hurtVignetteIntensity);

        }
        else if(i > 0 && player.GetHP() > halfMaxHP)
        {
            i -= Time.unscaledDeltaTime * this.ChnageRate;

            vignette.color = new ColorParameter(Color.Lerp(this.originVignetteColor, this.hurtVignetteColor, i));
            vignette.smoothness.value = Mathf.Clamp(i, originVignetteSmooth, hurtVignetteSmooth);
            vignette.intensity.value = Mathf.Clamp(i, originVignetteIntensity, hurtVignetteIntensity);

        }
    }


   
}
