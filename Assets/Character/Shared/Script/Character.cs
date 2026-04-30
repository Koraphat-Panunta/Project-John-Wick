using System;
using UnityEngine;


public abstract class Character : MonoBehaviour,IInitializedAble
{
    public abstract Gauge _hpGauge { get; protected set; } 

    public bool enableRootMotion;

    public HumanoidBone humanoidBone;

    [SerializeField] MovementScriptableObject movementScriptableObject;

    public float StandMoveAccelerate { get => this.movementScriptableObject.StandMoveAccelerate; }
    public virtual float StandMoveMaxSpeed { get => this.movementScriptableObject.StandMoveMaxSpeed; }

    public float CrouchMoveAccelerate { get => this.movementScriptableObject.CrouchMoveAccelerate; }
    public float CrouchMoveMaxSpeed { get => this.movementScriptableObject.CrouchMoveMaxSpeed; }

    public float rotateSpeed { get => this.movementScriptableObject.moveRotateSpeed; }

    public float sprintAccelerate { get => this.movementScriptableObject.sprintAccelerate; }
    public float sprintMaxSpeed { get => this.movementScriptableObject.sprintMaxSpeed; }
    public float sprintRotateSpeed { get => this.movementScriptableObject.sprintRotateSpeed; }

    public float breakDecelerate { get => this.movementScriptableObject.breakDecelerate; }

    public virtual bool isDead { get 
        {
            if(this._hpGauge._gauge <=0)
                return true;
            else return false;
        }
    }

    public abstract Stance stance { get; }

    public abstract MovementCompoent _movementCompoent { get; /*protected*/ set; }
    [SerializeField] public CharacterMovementController characterController;
    //public Weapon curentWeapon;
    //public Transform weaponSocket;
    public Animator animator;
    int frame;
    [SerializeField] private float SumDeltaPos;

    public virtual void HandleAnimatorMove(Vector3 deltaPosition, Quaternion deltaRotation)
    {
        if (!this.enableRootMotion) 
        {
            this.SumDeltaPos = 0;
            this.frame = 0;
            return; 
        }

        this.frame++;

        this.SumDeltaPos += animator.deltaPosition.magnitude;

        this._movementCompoent.SetPosition(transform.position + deltaPosition);
        this._movementCompoent.SetRotation(transform.rotation * deltaRotation);
    }
   

    public virtual void Initialized()
    {
        
    }
  
   
    public float GetHP()
    {
        return this._hpGauge._gauge;
    }
    public float GetMaxHp() => this._hpGauge.maxGauge;
    public void SetHP(float HP)
    {
        this._hpGauge.SetGauge(Mathf.Clamp(HP, 0, GetMaxHp()));
    }
    public void AddHP(float HP)
    {
        this._hpGauge.AddGauge(Math.Clamp(this.GetHP() + HP, 0, this.GetMaxHp()));
    }
   
  
}
