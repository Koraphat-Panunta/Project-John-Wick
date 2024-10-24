using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAudio : MonoBehaviour,IObserverWeapon
{
    public void OnNotify(Weapon weapon, WeaponSubject.WeaponNotifyType weaponNotify)
    {
        if(weaponNotify == WeaponSubject.WeaponNotifyType.Firing)
        {
            TriggerFiringSound();
        }
        if(weaponNotify == WeaponSubject.WeaponNotifyType.Reloading)
        {
            TriggerReloadSound();
        }
        if (weaponNotify == WeaponSubject.WeaponNotifyType.TacticalReload)
        {
            TriggerReloadSound();
        }
    }
    [SerializeField] private AudioSource source_Sound;

    [SerializeField] private AudioClip firing;
    [SerializeField] private AudioClip trigger;
    [SerializeField] private AudioClip reload_1;
    [SerializeField] private AudioClip reload_2;
    [SerializeField] private AudioClip reload_3;
    private Coroutine coroutine;
    public Weapon weapon;
    private void TriggerFiringSound()
    {
        if(coroutine != null)
        {
            StopCoroutine(ReloadSoundEvent());
        }
        source_Sound.PlayOneShot(firing);
    }
    private void TriggerReloadSound()
    {
        if (coroutine != null)
        {
            StopCoroutine(ReloadSoundEvent());
        }
        coroutine = StartCoroutine(ReloadSoundEvent());
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
    protected virtual void Start()
    {
        this.source_Sound = GetComponent<AudioSource>();
        weapon = GetComponent<Weapon>();
        weapon.AddObserver(this);
    }
    private void OnDisable()
    {
        weapon.Remove(this);
    }
}
