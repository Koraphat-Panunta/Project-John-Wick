using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IWeaponSenses,IDamageAble
{
    protected float HP;
    public event Action<Weapon> Aim;
    public event Action<Weapon> Fire;
    public event Action<Weapon> Reload;
    public event Action<Weapon> LowWeapon;
    //Call When Weapon performed Event
    public virtual void Aiming(Weapon weapon)
    {
        if (Aim != null)
        Aim.Invoke(weapon);
    }


    public virtual void Firing(Weapon weapon)
    {
        if (Fire != null)
        {
            Fire.Invoke(weapon);
        }
    }

    public virtual void LowReadying(Weapon weapon)
    {
        if (LowWeapon != null)
        LowWeapon.Invoke(weapon);
    }

    public virtual void Reloading(Weapon weapon, Reload.ReloadType reloadType)
    {
        if (Reload != null)
        {
            Reload.Invoke(weapon);
        }
    }
    

    public virtual void TakeDamage(float Damage)
    {
        HP -= Damage;
        Debug.Log("HP = " + HP);
    }
}
