using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAudio : MonoBehaviour,IObserverRangeWeapon,IInitializedAble
{
    public void OnNotify<T>(RangeWeapon weapon, T weaponNotify)
    {
        if(weaponNotify is FiringNode)
        {
            TriggerFiringSound();
        }
        if(weaponNotify is Action action
            && action == (weapon as MagazineType).ReleseMagazine)
        {
            if((weapon.userWeapon._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<ReloadMagazineFullStageNodeLeaf>())
                this.TriggerReloadSound();
            else if((weapon.userWeapon._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<TacticalReloadMagazineFullStageNodeLeaf>())
                this.TriggerTacticalReloadSound();
        }
        
    }
    [SerializeField] private AudioSource source_Sound;

    [SerializeField] private AudioClip trigger;
    [SerializeField] private AudioClip reload_1;
    [SerializeField] private AudioClip reload_2;
    [SerializeField] private AudioClip reload_3;
    private Coroutine coroutine;
    public RangeWeapon weapon;

    [SerializeField] protected SoundData firingData;

    private void TriggerFiringSound()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        SoundEmitterManager.Instance.CreateSoundBuilder(this.weapon.bulletSpawner.transform.position, this.firingData).Play();
    }
    private void TriggerReloadSound()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ReloadSoundEvent());
    }
    private void TriggerTacticalReloadSound()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(TacReloadSoundEvent());
    }
    protected abstract float reloadSoundWait_0 { get; set; }//Start -> เอาMagออก
    protected abstract float reloadSoundWait_1 { get; set; }//เอาMagออก -> ใส่Mag
    protected abstract float reloadSoundWait_2 { get; set; }//ใส่Mag -> ขึ้นลำ
    IEnumerator ReloadSoundEvent()
    {
        yield return new WaitForSeconds(reloadSoundWait_0);
        source_Sound.PlayOneShot(reload_1);
        yield return new WaitForSeconds(reloadSoundWait_1);
        source_Sound.PlayOneShot(reload_2);
        yield return new WaitForSeconds(reloadSoundWait_2);
        source_Sound.PlayOneShot(reload_3);
    }
    IEnumerator TacReloadSoundEvent()
    {
        yield return new WaitForSeconds(reloadSoundWait_0);
        source_Sound.PlayOneShot(reload_1);
        yield return new WaitForSeconds(reloadSoundWait_1);
        source_Sound.PlayOneShot(reload_2);
       
    }
    public virtual void Initialized()
    {
        this.source_Sound = GetComponent<AudioSource>();
        weapon = GetComponent<RangeWeapon>();
        weapon.AddObserver(this);
    }
}
