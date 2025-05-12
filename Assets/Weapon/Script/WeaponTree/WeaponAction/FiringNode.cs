using System;
using System.Collections.Generic;
using UnityEngine;

public class FiringNode : WeaponLeafNode
{
    bool isFiring;

    public FiringNode(Weapon weapon, Func<bool> preCondition) : base(weapon, preCondition)
    {
    }

    public override void Enter()
    {
       isFiring = false;
       Weapon.bulletStore[BulletStackType.Chamber] -= 1;
       Weapon.bulletSpawner.SpawnBullet(Weapon);
       Weapon.Notify(Weapon, WeaponSubject.WeaponNotifyType.Firing);
       Weapon.userWeapon.weaponAfterAction.Firing(Weapon);
       isFiring = true;
    }

    public override void Exit()
    {
       
    }


    public override void UpdateNode()
    {

    }
    public override void FixedUpdateNode()
    {
        
    }

    public override bool IsComplete()
    {
        return isFiring;
    }

    public override bool IsReset()
    {
        return IsComplete();
    }

    //public override bool Precondition()
    //{
    //    return Weapon.bulletStore[BulletStackType.Chamber] > 0 ;
    //}

   
    
}
