using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour,IInitializedAble
{
    protected float HP;
    protected float maxHp;
    public bool enableRootMotion;
    public virtual bool isDead { get 
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
            _movementCompoent.SetPosition(transform.position + animator.deltaPosition);
            _movementCompoent.SetRotation(transform.rotation * animator.deltaRotation);
        }

    }

    public virtual void Initialized()
    {
        
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
