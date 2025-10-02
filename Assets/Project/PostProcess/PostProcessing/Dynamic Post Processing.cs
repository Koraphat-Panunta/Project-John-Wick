using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DynamicPostProcessing : MonoBehaviour,IInitializedAble
{
    [SerializeField] protected Player player;
    protected INodeManager playerStateNodeManager => player.playerStateNodeManager as INodeManager;
    [SerializeField] protected TimeControlManager timeControlManager;
    [SerializeField] protected Volume volume;

    protected Vignette vignette;
    private float originVignetteIntensity;
    private Color originVignetteColor;
    private float originVignetteSmooth;
    [Range(0,1)]
    [SerializeField] private float hurtVignetteIntensity;
    [SerializeField] private Color hurtVignetteColor;
    [Range(0, 1)]
    [SerializeField] private float hurtVignetteSmooth;

    protected ChromaticAberration chromaticAberration;
    private float originChromaticAberration;
    [Range(0, 1)]
    [SerializeField] private float bulletTimeChromaticAberration;

    public void Initialized()
    {

        if (this.volume != null)
        {
            this.volume.profile.TryGet<Vignette>(out this.vignette);
            this.volume.profile.TryGet<ChromaticAberration>(out this.chromaticAberration);

            originChromaticAberration = this.chromaticAberration.intensity.value;

            originVignetteColor = this.vignette.color.value;
            originVignetteIntensity = this.vignette.intensity.value;
            originVignetteSmooth = this.vignette.smoothness.value;
        }
    }
    private void Update()
    {
        this.HurtEffectCheck();
        this.BulletTimeEffect();
    }
    private void OnValidate()
    {
        this.player = FindAnyObjectByType<Player>();
        this.timeControlManager = FindAnyObjectByType<TimeControlManager>();

       
    }

    float halfMaxHP => player.GetMaxHp() * 0.5f;
    float i = 0;
    private float ChnageRate = 1;
    [SerializeField] AnimationCurve hurtFeedBackIntensityCurve;
    private void HurtEffectCheck() 
    {
       
        if (player.GetHP() <= halfMaxHP)
        {
            float targetI = Mathf.Clamp01(player.GetHP() / halfMaxHP);

            i = Mathf.MoveTowards(i, 1 - targetI, Time.unscaledDeltaTime * this.ChnageRate);

            vignette.color.value = Color.Lerp(this.originVignetteColor, this.hurtVignetteColor, hurtFeedBackIntensityCurve.Evaluate(i));
            vignette.smoothness.value = Mathf.Clamp(hurtFeedBackIntensityCurve.Evaluate(i), originVignetteSmooth,hurtVignetteSmooth);
            vignette.intensity.value = Mathf.Clamp(hurtFeedBackIntensityCurve.Evaluate(i), originVignetteIntensity,hurtVignetteIntensity);

        }
        else if(i > 0 && player.GetHP() > halfMaxHP)
        {
            i -= Time.unscaledDeltaTime * this.ChnageRate;

            vignette.color.value = Color.Lerp(vignette.color.value, this.hurtVignetteColor,Time.unscaledDeltaTime * this.ChnageRate);
            vignette.smoothness.value = Mathf.Clamp(vignette.smoothness.value, originVignetteSmooth, Time.unscaledDeltaTime * this.ChnageRate);
            vignette.intensity.value = Mathf.Clamp(vignette.intensity.value, originVignetteIntensity, Time.unscaledDeltaTime * this.ChnageRate);

        }
    }

    private void BulletTimeEffect()
    {
        if((playerStateNodeManager.TryGetCurNodeLeaf<RestrictGunFuStateNodeLeaf>()
            ||playerStateNodeManager.TryGetCurNodeLeaf<HumanShield_GunFuInteraction_NodeLeaf>())
            &&(timeControlManager.triggerBulletTime.timer > 0)) 
        {
            chromaticAberration.intensity.value = Mathf.MoveTowards(chromaticAberration.intensity.value, bulletTimeChromaticAberration, Time.unscaledDeltaTime);
        }
        else
        {
            chromaticAberration.intensity.value = Mathf.MoveTowards(chromaticAberration.intensity.value, originChromaticAberration, Time.unscaledDeltaTime);
        }

        Debug.Log("timeControlManager.triggerBulletTime.timer = " + timeControlManager.triggerBulletTime.timer);
    }


   
}
