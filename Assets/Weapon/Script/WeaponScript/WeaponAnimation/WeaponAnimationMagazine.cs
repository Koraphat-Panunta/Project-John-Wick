using UnityEngine;
using System;
using System.Collections;

public class WeaponAnimationMagazine : WeaponAnimation
{
    [SerializeField] public GameObject magazine;

    [SerializeField] protected SkinnedMeshRenderer muzzleRenderer;
    [SerializeField] protected SkinnedMeshRenderer sightRenderer;

    [SerializeField] private string ReloadMagazineFullStage;
    [SerializeField] private string TacticalReloadMagazineFullStage;
    [SerializeField] private string Rest;

    [SerializeField] private string OpenChamber;
    [SerializeField] private string CloseChamber;
    [SerializeField] private string FiringMechanic;
    public override void OnNotify(Weapon weapon, WeaponSubject.WeaponNotifyType weaponNotify)
    {
       if(weaponNotify == WeaponSubject.WeaponNotifyType.ReloadMagazineFullStage)
       {
            animator.CrossFade(ReloadMagazineFullStage, 0f, 0);
        }
       else if(weaponNotify == WeaponSubject.WeaponNotifyType.TacticalReloadMagazineFullStage)
       {
            animator.CrossFade(TacticalReloadMagazineFullStage,0f, 0);
       }

       if(weaponNotify == WeaponSubject.WeaponNotifyType.Firing) 
        {
            if (weapon.bulletStore[BulletStackType.Magazine] <= 0)
                animator.CrossFade(OpenChamber, 0.1f, 1);
            else
                animator.CrossFade(FiringMechanic, 0.1f, 1);
        }

       if(weaponNotify == WeaponSubject.WeaponNotifyType.Rest)
        {
            animator.CrossFade(Rest, 0f, 0);
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
        animator.CrossFade(CloseChamber, 0.1f, 1);
    }
    public void SpawnMagDrop()
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
        //if (weapon.muzzle != null) muzzleRenderer.enabled = false;
        //else muzzleRenderer.enabled = true;
        //if (weapon.Sight != null) sightRenderer.enabled = false;
        //else sightRenderer.enabled = true;
    }
}
