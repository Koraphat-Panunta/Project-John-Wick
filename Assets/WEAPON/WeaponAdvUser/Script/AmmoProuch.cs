using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoProuch 
{
    protected Dictionary<BulletType, int> amountOf_ammo = new Dictionary<BulletType, int>();
    protected Dictionary<BulletType, int> maximunAmmo = new Dictionary<BulletType, int>();



    public AmmoProuch() 
    {
        this.maximunAmmo.Add(BulletType.handgunAmmo, 24);
        this.maximunAmmo.Add(BulletType.rifleAmmo, 30);
        this.maximunAmmo.Add(BulletType.buckShotAmmo, 8);
        this.maximunAmmo.Add(BulletType.battleRifleAmmo, 20);

        this.amountOf_ammo.Add(BulletType.handgunAmmo, 24);
        this.amountOf_ammo.Add(BulletType.rifleAmmo, 30);
        this.amountOf_ammo.Add(BulletType.buckShotAmmo, 0);
        this.amountOf_ammo.Add(BulletType.battleRifleAmmo, 0);
    }

    public void SetMaximunAmmo(BulletType bulletType,int maxAmout)
    {
        this.maximunAmmo[bulletType] = maxAmout;
    }
    public void AddAmmo(BulletType bulletType,int amount)
    {
        if (this.amountOf_ammo[bulletType] + amount > this.maximunAmmo[bulletType])
        {
            amount = amount - ((this.amountOf_ammo[bulletType] + amount) - this.maximunAmmo[bulletType]);
        }
        this.amountOf_ammo[bulletType] += amount;
    }
    public void ForceAddAmmo(BulletType bulletType,int amout)
    {
        this.amountOf_ammo[bulletType] += amout;
    }
    public void SetAmmo(BulletType bulletType,int amout)
    {
        this.amountOf_ammo[bulletType] = amout;
    }

    public void GetAmmoOut(BulletType bulletType,int getAmout,out int outAmout)
    {
        outAmout = Mathf.Clamp(getAmout,0, this.amountOf_ammo[bulletType]);
        this.amountOf_ammo[bulletType] -= outAmout; 
    }

    public int CheckMaxAmmo(BulletType bulletType)
    {
        return this.maximunAmmo[bulletType];
    }

    public int CheckAmmo(BulletType bulletType) => this.amountOf_ammo[bulletType];

}
