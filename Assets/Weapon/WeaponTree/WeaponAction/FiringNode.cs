using System.Collections.Generic;
using UnityEngine;

public class FiringNode : WeaponActionNode
{
    bool isFiring;
    public FiringNode(Weapon weapon):base(weapon) 
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

    public override void FixedUpdate()
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

    public override bool PreCondition()
    {
        return Weapon.bulletStore[BulletStackType.Chamber] > 0 ;
    }

    public override void Update()
    {
        
    }
}
