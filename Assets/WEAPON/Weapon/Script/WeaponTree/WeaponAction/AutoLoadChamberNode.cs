using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLoadChamberNode : WeaponLeafNode
{
    private Coroutine reChamber;
    private Chamber chamber => this.Weapon.chamber;
    private BulletCapacity bulletCapacity => this.Weapon.bulletCap;

    public AutoLoadChamberNode(Weapon weapon, Func<bool> preCondition) : base(weapon, preCondition)
    {
    }

    public override void Enter()
    {
        this.reChamber = this.Weapon.StartCoroutine(ReChambering());
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
        return this.reChamber == null;
    }
    public IEnumerator ReChambering()
    {
        yield return new WaitForSeconds((float)(60 / Weapon.rate_of_fire));
        
        if(this.bulletCapacity.GetBulletOut(out Bullet bullet))
        {
            this.chamber.Load(bullet);
        }

        this.reChamber = null;
    }

}
