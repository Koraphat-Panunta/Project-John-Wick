using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAnimation : MonoBehaviour,IObserverWeapon,IInitializedAble
{
    [SerializeField] public Weapon weapon;
    [SerializeField] public Animator animator;


    // Start is called before the first frame update
    public void Initialized()
    {
        weapon.AddObserver(this);
    }
  
    public abstract void OnNotify(Weapon weapon, WeaponSubject.WeaponNotifyType weaponNotify);

    
}
