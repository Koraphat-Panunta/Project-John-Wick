using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public abstract class Weapon : WeaponSubject 
{
    public WeaponStateManager weapon_stateManager { get; protected set; }
    public WeaponStanceManager weapon_StanceManager { get; protected set; }
    public int Magazine_count;
    public int Chamber_Count;
    public Transform bulletSpawnerPos;
    public abstract int Magazine_capacity { get; protected set; }
    public abstract float rate_of_fire { get; protected set; }
    public abstract float reloadSpeed { get; protected set; }
    public abstract float Accuracy { get; protected set; }
    public abstract float RecoilController { get; protected set; }
    public abstract float RecoilCameraKickBack {  get; protected set; }
    public abstract float aimDownSight_speed { get; protected set; }
    public abstract GameObject bullet { get; protected set; }
    public abstract float RecoilKickBack { get; protected set; }
    public abstract float min_Precision { get; protected set; }
    public abstract float max_Precision { get; protected set; }

    public Character userWeapon;
    public ParentConstraint parentConstraint;
    public Rigidbody rb;
    public AnimatorOverrideController _weaponOverrideControllerPlayer;
    public AnimatorOverrideController _weaponOverrideControllerEnemy;
    public BulletSpawner bulletSpawner;
    public enum FireMode
    {
        Single,
        //Burst,
        FullAuto
    }
    public FireMode fireMode { get; protected set; }
    public enum TriggerPull
    {
        Up,
        IsDown,
        Down,
        IsUp,
    }
    public TriggerPull triggerPull = TriggerPull.Up;
   
    protected virtual void Start()
    {
        weapon_stateManager = new WeaponStateManager(this);
        weapon_StanceManager = new WeaponStanceManager(this);
        parentConstraint = GetComponent<ParentConstraint>();
        rb = GetComponent<Rigidbody>();
        Magazine_count = Magazine_capacity;
    }
    protected virtual void Update()
    {
        if (userWeapon != null)
        {
            weapon_StanceManager.Update();
            weapon_stateManager.Update();
        }
    }
    protected virtual void FixedUpdate()
    {
        if (userWeapon != null)
        {
            weapon_StanceManager.FixedUpdate();
            weapon_stateManager.FixedUpdate();
        }
    }
    public virtual void Aim()
    {
        weapon_StanceManager.ChangeStance(weapon_StanceManager.aimDownSight);
    }
    public virtual void Fire() 
    {
        if (fireMode == FireMode.Single)
        {
            if(triggerPull == TriggerPull.IsDown)
            {
                weapon_stateManager.ChangeState(weapon_stateManager.fireState);
            }
        }
        if(fireMode == FireMode.FullAuto)
        {
            if(triggerPull == TriggerPull.IsDown||triggerPull == TriggerPull.Down)
            {
                weapon_stateManager.ChangeState(weapon_stateManager.fireState);
            }
        }
    }
    public virtual void Reload() 
    {
        weapon_stateManager.ChangeState(weapon_stateManager.reloadState);
    }
    public virtual void LowWeapon()
    {
        weapon_StanceManager.ChangeStance(weapon_StanceManager.lowReady);
    }
    public void AttatchWeaponTo(Character WeaponUser)
    {
        this.userWeapon = WeaponUser;
        WeaponUser.curentWeapon = this;
        rb.isKinematic = true;
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = WeaponUser.weaponSocket;
        source.weight = 1;
        if (parentConstraint.sourceCount > 0)
        {
            parentConstraint.RemoveSource(0);
        }
        parentConstraint.AddSource(source);
        parentConstraint.constraintActive = true;
        parentConstraint.translationAtRest = Vector3.zero;
        parentConstraint.rotationAtRest = Vector3.zero;
        parentConstraint.constraintActive = true;
        if(WeaponUser.TryGetComponent<Player>(out Player p))
        {
            p.animator.runtimeAnimatorController = _weaponOverrideControllerPlayer;
        }
        if(WeaponUser.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.animator.runtimeAnimatorController = _weaponOverrideControllerEnemy;
        }
        parentConstraint.weight = 1;
    }
    public void AttachWeaponTo(Transform weaponSocket)
    {
        userWeapon = null;
        rb.isKinematic = true;
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = weaponSocket;
        source.weight = 1;
        if (parentConstraint.sourceCount > 0)
        {
            parentConstraint.RemoveSource(0);
        }
        parentConstraint.AddSource(source);
        parentConstraint.constraintActive = true;
        parentConstraint.translationAtRest = Vector3.zero;
        parentConstraint.rotationAtRest = Vector3.zero;
        parentConstraint.constraintActive = true;
        //if (WeaponUser.TryGetComponent<Player>(out Player p))
        //{
        //    p.animator.runtimeAnimatorController = _weaponOverrideControllerPlayer;
        //}
        //if (WeaponUser.TryGetComponent<Enemy>(out Enemy enemy))
        //{
        //    enemy.animator.runtimeAnimatorController = _weaponOverrideControllerEnemy;
        //}
        parentConstraint.weight = 1;
    }
    public void DropWeapon()
    {
        rb.isKinematic = false;
    }

}
