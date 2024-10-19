using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour,IObserverWeapon
{
    [SerializeField] public Weapon weapon;
    [SerializeField] public Animator animator;

    public void OnNotify(Weapon weapon, WeaponSubject.WeaponNotifyType weaponNotify)
    {
        Debug.Log("WeaponRecived Notify");
       if(weaponNotify == WeaponSubject.WeaponNotifyType.Reloading)
       {
            animator.SetTrigger("Reloading");
       }
       else if(weaponNotify == WeaponSubject.WeaponNotifyType.TacticalReload)
       {
            animator.SetTrigger("Reloading");
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
}
