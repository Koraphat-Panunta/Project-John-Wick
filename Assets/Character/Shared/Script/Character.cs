using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour,IInitializedAble
{
    protected float HP;
    protected float maxHp;
    public bool enableRootMotion;

    public Transform _hipBone;
    public Transform _leftUpperLegBone;
    public Transform _leftLowerLegBone;
    public Transform _leftFootBone;
    public Transform _rightUpperLegBone;
    public Transform _rightLowerLegBone;
    public Transform _rightFootBone;
    public Transform _spine_0_Bone;
    public Transform _spine_1_Bone;
    public Transform _spine_2_Bone;
    public Transform _leftShoulderBone;
    public Transform _leftArmBone;
    public Transform _leftForeArmBone;
    public Transform _leftHandBone;

    public Transform _neckBone;
    public Transform _headBone;

    public Transform _rightShoulderBone;
    public Transform _rightArmBone;
    public Transform _rightForeArmBone;
    public Transform _rightHandBone;



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
