using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAnimation : MonoBehaviour,IObserverWeapon
{
    [SerializeField] public Weapon weapon;
    [SerializeField] public Animator animator;
  

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

    public abstract void OnNotify(Weapon weapon, WeaponSubject.WeaponNotifyType weaponNotify);
  
}
