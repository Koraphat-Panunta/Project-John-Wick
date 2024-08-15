using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoProuch 
{
    
    public Dictionary<BulletType,int> amountOf_ammo = new Dictionary<BulletType,int>();
    public AmmoProchReload prochReload;
    public AmmoProuch(int start9mm,int start45mm,int start556mm,int start762mm) 
    {
        amountOf_ammo.Add(BulletType._9mm, start9mm);
        amountOf_ammo.Add(BulletType._45mm, start45mm);
        amountOf_ammo.Add(BulletType._556mm, start556mm);
    }

}
