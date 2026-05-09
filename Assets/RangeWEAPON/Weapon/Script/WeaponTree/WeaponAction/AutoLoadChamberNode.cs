using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLoadChamberNode : WeaponLeafNode
{
    private Chamber chamber => this.Weapon.chamber;
    private BulletCapacity bulletCapacity => this.Weapon.TryGetBulletCapacity(out BulletCapacity bulletCapacity)?bulletCapacity:null;
    float timer;
    float rechamberTime => ((float)(60 / Weapon.rate_of_fire));
    public AutoLoadChamberNode(RangeWeapon weapon, Func<bool> preCondition) : base(weapon, preCondition)
    {
    }

    public override void Enter()
    {
        this.timer = 0;
    }

    public override void Exit()
    {

    }

    public override void UpdateNode()
    {
        Debug.Log("AutoLoadChamberNode UpdateNode " + this.timer);
        this.timer += Time.deltaTime;
        if(this.timer >= this.rechamberTime)
        {
            this.ReChambering();
        }
    }
    public override void FixedUpdateNode()
    {

    }
    public override bool IsReset()
    {
        if(IsComplete())
            return true;

        return this.timer >= this.rechamberTime;
    }

    public override bool IsComplete()
    {
        return this.chamber.isReadyShoot;
    }
    public void ReChambering()
    {

        this.chamber.UnLoad();
        if (this.bulletCapacity != null
            &&this.bulletCapacity.GetBulletOut(out Bullet bullet)
            )
        {
            //Debug.Log("Auto load Chamber "+this.RangeWeapon);
            this.chamber.Load(bullet);
            //Debug.Log(this.RangeWeapon + "isReadyShoot == "+this.chamber.isReadyShoot);
        }
    }

}
