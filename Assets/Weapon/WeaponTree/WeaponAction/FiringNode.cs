using System.Collections.Generic;
using UnityEngine;

public class FiringNode : WeaponActionNode
{
    //public override List<WeaponNode> SubNode { get; set ; }
    private Weapon weapon;
    //bool isFiring;
    public FiringNode(WeaponTreeManager weaponTree):base(weaponTree) 
    {
        weapon = weaponTree.weapon;
    }
    public override void Enter()
    {

       weapon.bulletStore[BulletStackType.Chamber] -= 1;
       weapon.bulletSpawner.SpawnBullet(weapon);
       weapon.Notify(weapon, WeaponSubject.WeaponNotifyType.Firing);
       weapon.userWeapon.weaponAfterAction.Firing(weapon);
       //isFiring = true;
    }

    public override void Exit()
    {
       
    }

    public override void FixedUpdate()
    {
       
    }

    public override bool IsComplete()
    {
        //Debug.Log("Is FiringComplete");
        return true;
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
        //Debug.Log("Fiing Updare");
    }
}
