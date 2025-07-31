using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected float HP;
    protected float maxHp;
    public bool enableRootMotion;
    public bool isDead { get 
        {
            if(HP <=0)
                return true;
            else return false;
        }
    }

    public abstract MovementCompoent _movementCompoent { get; /*protected*/ set; }
    //public Weapon curentWeapon;
    //public Transform weaponSocket;
    public Animator animator;
    private void OnAnimatorMove()
    {
        if (enableRootMotion)
        {
            _movementCompoent.Move(animator.deltaPosition);
            transform.rotation *= animator.deltaRotation;
        }

    }
    protected virtual void Start()
    {

    }
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public float GetHP()
    {
        return HP;
    }
    public float GetMaxHp() => maxHp;
    public void SetHP(float HP)
    {
        this.HP = Mathf.Clamp(HP,0,GetMaxHp());
    }
    public void AddHP(float HP)
    {
        this.HP = Math.Clamp(this.HP+HP, 0, this.maxHp);
        
    }

   
}
