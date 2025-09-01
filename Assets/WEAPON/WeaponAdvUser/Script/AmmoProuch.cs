using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoProuch 
{
    public Dictionary<BulletType,int> amountOf_ammo = new Dictionary<BulletType,int>();
    public Dictionary<BulletType,int> maximunAmmo = new Dictionary<BulletType,int>();


    public AmmoProuch(int start9mm,int start45mm,int start556mm,int start762mm,
        int max9mm,int max45mm,int max556mm,int max762mm) 
    {
        maximunAmmo.Add(BulletType._9mm, max9mm);
        maximunAmmo.Add(BulletType._45mm, max45mm);
        maximunAmmo.Add(BulletType._556mm, max556mm);
        maximunAmmo.Add(BulletType._762mm, max762mm);

        start9mm = Mathf.Clamp(start9mm, 0, maximunAmmo[BulletType._9mm]);
        start45mm = Mathf.Clamp(start45mm, 0, maximunAmmo[BulletType._45mm]);
        start556mm = Mathf.Clamp(start556mm, 0, maximunAmmo[BulletType._556mm]);
        start762mm = Mathf.Clamp(start762mm, 0, maximunAmmo[BulletType._762mm]);

        amountOf_ammo.Add(BulletType._9mm, start9mm);
        amountOf_ammo.Add(BulletType._45mm, start45mm);
        amountOf_ammo.Add(BulletType._556mm, start556mm);
        amountOf_ammo.Add(BulletType._762mm, start762mm);
    }
    public void AddAmmo(BulletType bulletType,int amount)
    {
        amountOf_ammo[bulletType] = Mathf.Clamp(amountOf_ammo[bulletType] + amount, 0, maximunAmmo[bulletType]);
    }
    public void SetAmmo(BulletType bulletType,int amout)
    {
        amountOf_ammo[bulletType] = amout;
    }
    public void AddAmmo( int amount)
    {
        amountOf_ammo[BulletType._9mm] = Mathf.Clamp(amountOf_ammo[BulletType._9mm] + amount, 0, maximunAmmo[BulletType._9mm]);
        amountOf_ammo[BulletType._45mm] = Mathf.Clamp(amountOf_ammo[BulletType._45mm] + amount, 0, maximunAmmo[BulletType._45mm]);
        amountOf_ammo[BulletType._556mm] = Mathf.Clamp(amountOf_ammo[BulletType._556mm] + amount, 0, maximunAmmo[BulletType._556mm]);
        amountOf_ammo[BulletType._762mm] = Mathf.Clamp(amountOf_ammo[BulletType._762mm] + amount, 0, maximunAmmo[BulletType._762mm]);
    }
    public void SetAmmo( int amount)
    {
        amountOf_ammo[BulletType._9mm] = Mathf.Clamp(amount, 0, maximunAmmo[BulletType._9mm]);
        amountOf_ammo[BulletType._45mm] = Mathf.Clamp(amount, 0, maximunAmmo[BulletType._45mm]);
        amountOf_ammo[BulletType._556mm] = Mathf.Clamp(amount, 0, maximunAmmo[BulletType._556mm]);
        amountOf_ammo[BulletType._762mm] = Mathf.Clamp(amount, 0, maximunAmmo[BulletType._762mm]);
    }

}
