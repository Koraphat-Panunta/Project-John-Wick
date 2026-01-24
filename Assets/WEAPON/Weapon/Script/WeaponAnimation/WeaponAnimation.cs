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
    private void OnValidate()
    {
        if (weapon == null)
        {
            weapon = GetComponent<Weapon>();
        }
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }
    public abstract void OnNotify<T>(Weapon weapon, T weaponNotify);


}
