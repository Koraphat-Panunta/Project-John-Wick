using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoProuch 
{
    public Dictionary<BulletType,int> amountOf_ammo = new Dictionary<BulletType,int>();
    public Dictionary<BulletType,int> maximunAmmo = new Dictionary<BulletType,int>();

    private int max_9mmDefault = 90;
    private int max_45mmDefault = 90;
    private int max_556mmDefault = 360;
    private int max_762mmDefault = 360;
    public AmmoProuch(int start9mm,int start45mm,int start556mm,int start762mm) 
    {
        maximunAmmo.Add(BulletType._9mm,max_9mmDefault);
        maximunAmmo.Add(BulletType._45mm, max_45mmDefault);
        maximunAmmo.Add(BulletType._556mm, max_556mmDefault);
        maximunAmmo.Add(BulletType._762mm, max_762mmDefault);

        start9mm = Mathf.Clamp(start9mm, 0, maximunAmmo[BulletType._9mm]);
        start45mm = Mathf.Clamp(start9mm, 0, maximunAmmo[BulletType._45mm]);
        start556mm = Mathf.Clamp(start9mm, 0, maximunAmmo[BulletType._556mm]);
        start762mm = Mathf.Clamp(start9mm, 0, maximunAmmo[BulletType._762mm]);

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

}
