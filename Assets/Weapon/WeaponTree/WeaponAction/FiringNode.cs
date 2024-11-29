using System.Collections.Generic;
using UnityEngine;

public class FiringNode : WeaponActionNode
{
    //public override List<WeaponNode> SubNode { get; set ; }
    private Weapon weapon;
    bool isFiring;
    public FiringNode(WeaponTreeManager weaponTree):base(weaponTree) 
    {
        
    }
    public override void Enter()
    {
       isFiring = false;
       weapon.bulletStore[BulletStackType.Chamber] -= 1;
       weapon.bulletSpawner.SpawnBullet(weapon);
       weapon.Notify(weapon, WeaponSubject.WeaponNotifyType.Firing);
       weapon.userWeapon.weaponAfterAction.Firing(weapon);
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
        return blackBoard.BulletStack[BulletStackType.Chamber] > 0;
    }

    public override void Update()
    {
        
    }
}
