using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IDamageAble
{
    protected float HP;

    public Environment My_environment;
    public bool isDead { get; set; }
   

    //public Weapon curentWeapon;
    //public Transform weaponSocket;
    public Animator animator;

    private void Start()
    {
       
    }
    private void Awake()
    {
        My_environment = FindAnyObjectByType<Environment>();

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
