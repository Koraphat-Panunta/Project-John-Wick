using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoProuch 
{
    protected Dictionary<BulletType, int> amountOf_ammo = new Dictionary<BulletType, int>();
    protected Dictionary<BulletType, int> maximunAmmo = new Dictionary<BulletType, int>();

    public struct MaximunAmmo
    {
        public Dictionary<BulletType, int> amountOf_;
    }
    public static Dictionary<int, MaximunAmmo> maximunAmmoTierData = new Dictionary<int, MaximunAmmo>()
    {
        {
            1, new MaximunAmmo
            {
                amountOf_ = new Dictionary<BulletType, int>()
                {
                    {BulletType.handgunAmmo, 24},
                    {BulletType.rifleAmmo,30 },
                    {BulletType.buckShotAmmo,8 },
                    {BulletType.battleRifleAmmo,10 }
                }
            }
        },
        {
            2, new MaximunAmmo
            {
                amountOf_ = new Dictionary<BulletType, int>()
                {
                    {BulletType.handgunAmmo, 32},
                    {BulletType.rifleAmmo,45 },
                    {BulletType.buckShotAmmo,12 },
                    {BulletType.battleRifleAmmo,15 }
                }
            }
        }, 
        {
            3, new MaximunAmmo
            {
                amountOf_ = new Dictionary<BulletType, int>()
                {
                    {BulletType.handgunAmmo, 48},
                    {BulletType.rifleAmmo,60 },
                    {BulletType.buckShotAmmo,16 },
                    {BulletType.battleRifleAmmo,20}
                }
            }
        },
    };
    public int ammoProuchTier { get; protected set; }

    public AmmoProuch() 
    {
        this.ammoProuchTier = 1;

        this.maximunAmmo.Add(BulletType.handgunAmmo, 24);
        this.maximunAmmo.Add(BulletType.rifleAmmo, 30);
        this.maximunAmmo.Add(BulletType.buckShotAmmo, 8);
        this.maximunAmmo.Add(BulletType.battleRifleAmmo, 20);

        this.amountOf_ammo.Add(BulletType.handgunAmmo, 24);
        this.amountOf_ammo.Add(BulletType.rifleAmmo, 30);
        this.amountOf_ammo.Add(BulletType.buckShotAmmo, 8);
        this.amountOf_ammo.Add(BulletType.battleRifleAmmo, 20);

        this.SetMaximumAmmoTier(this.ammoProuchTier);
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

    public void SetMaximumAmmoTier(int tier)
    {
        this.SetMaximunAmmo(BulletType.handgunAmmo, maximunAmmoTierData[tier].amountOf_[BulletType.handgunAmmo]);
        this.SetMaximunAmmo(BulletType.rifleAmmo, maximunAmmoTierData[tier].amountOf_[BulletType.rifleAmmo]);
        this.SetMaximunAmmo(BulletType.buckShotAmmo, maximunAmmoTierData[tier].amountOf_[BulletType.buckShotAmmo]);
        this.SetMaximunAmmo(BulletType.battleRifleAmmo, maximunAmmoTierData[tier].amountOf_[BulletType.battleRifleAmmo]);
    }
    public void SetAmmoProuchTier(int tier) => this.ammoProuchTier = tier;

    public void RefillAmmo()
    {
        this.SetAmmo(BulletType.handgunAmmo, this.maximunAmmo[BulletType.handgunAmmo]);
        this.SetAmmo(BulletType.rifleAmmo, this.maximunAmmo[BulletType.rifleAmmo]);
        this.SetAmmo(BulletType.buckShotAmmo, this.maximunAmmo[BulletType.buckShotAmmo]);
        this.SetAmmo(BulletType.battleRifleAmmo, this.maximunAmmo[BulletType.battleRifleAmmo]);
    }

}
