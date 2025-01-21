using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour,IObserverWeapon
{
    [SerializeField] public Weapon weapon;
    [SerializeField] public Animator animator;
    [SerializeField] public GameObject magazine;

    [SerializeField] protected SkinnedMeshRenderer muzzleRenderer;
    [SerializeField] protected SkinnedMeshRenderer sightRenderer;
    public void OnNotify(Weapon weapon, WeaponSubject.WeaponNotifyType weaponNotify)
    {
       if(weaponNotify == WeaponSubject.WeaponNotifyType.ReloadMagazineFullStage)
       {
            animator.CrossFade("ReloadMagazineFullStage", 0.1f, 0);
        }
       else if(weaponNotify == WeaponSubject.WeaponNotifyType.TacticalReloadMagazineFullStage)
       {
            animator.CrossFade("TacticalReloadMagazineFullStage", 0.1f, 0);
        }

       if(weaponNotify == WeaponSubject.WeaponNotifyType.Firing) 
        {
            if (weapon.bulletStore[BulletStackType.Magazine] <= 0)
                animator.CrossFade("BarrelOpen", 0.1f, 1);
            else
                animator.CrossFade("BarrelFiring", 0.1f, 1);
        }


        if (weaponNotify == WeaponSubject.WeaponNotifyType.AttachmentSetup)
        {
            SetWeaponApprerance(weapon);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(weapon == null)
        {
            weapon = GetComponent<Weapon>();
        }
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
        weapon.AddObserver(this);
    }
    private void OnDisable()
    {
        weapon.Remove(this);
    }
    [SerializeField] SkinnedMeshRenderer magMain;
    [SerializeField] SkinnedMeshRenderer magSecond;
    public void EnableSecondMag()
    {
        magSecond.enabled = true;
    }
    public void EnableMainMag()
    {
        magMain.enabled = true;
    }
    public void DisableSecondMag()
    {
        magSecond.enabled =false;
    }
    public void DisableMainMag()
    {
        magMain.enabled=false;
    }
    public void BarrelClose()
    {
        animator.CrossFade("BarrelClose", 0.1f, 1);
    }
    public virtual void SpawnMagDrop()
    {
        if (magazine != null)
        {
            GameObject mag = GameObject.Instantiate(magazine);
            mag.transform.position = gameObject.transform.position + Vector3.down * 0.1f;
            mag.GetComponent<Rigidbody>().AddForce(gameObject.transform.right * 5 + gameObject.transform.up * 4, ForceMode.Impulse);
            mag.GetComponent<Rigidbody>().AddForceAtPosition(mag.transform.right * 4, magazine.transform.position - magazine.transform.up * magazine.transform.localScale.y);
            StartCoroutine(Removemag(mag));
        }
    }
    protected IEnumerator Removemag(GameObject mag)
    {
        yield return new WaitForSeconds(3);
        GameObject.Destroy(mag);
    }
    protected virtual void SetWeaponApprerance(Weapon weapon)
    {
       if(weapon.muzzle != null) muzzleRenderer.enabled = false; 
        else muzzleRenderer.enabled = true;
       if(weapon.Sight != null) sightRenderer.enabled = false;
        else sightRenderer.enabled = true;
    }
}
