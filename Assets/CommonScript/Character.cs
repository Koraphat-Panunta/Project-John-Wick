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

    public Environment My_environment;
    public bool isDead { get; set; }
    public Weapon curentWeapon;
    public Transform weaponSocket;
    public Animator animator;

    private void Start()
    {
       
    }
    private void Awake()
    {
        My_environment = FindAnyObjectByType<Environment>();

    }
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
    }
    public float GetHP()
    {
        return HP;
    }
    public void SetHP(float HP)
    {
        this.HP = HP;
    }
    public void AddHP(float HP)
    {
        this.HP += HP;
    }
}
