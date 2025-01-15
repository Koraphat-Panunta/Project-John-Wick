using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected float HP;

    public Environment My_environment;
    public bool isDead { get 
        {
            if(HP <=0)
                return true;
            else return false;
        }
    }
   

    //public Weapon curentWeapon;
    //public Transform weaponSocket;
    public Animator animator;

    protected virtual void Start()
    {
       
    }
    private void Awake()
    {
        My_environment = FindAnyObjectByType<Environment>();

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
