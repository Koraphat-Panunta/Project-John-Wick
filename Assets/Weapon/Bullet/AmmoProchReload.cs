using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoProchReload : IAmmoProchAction
{
    private AmmoProuch ammoProuch;
    public AmmoProchReload(AmmoProuch ammoProuch) 
    {
        this.ammoProuch = ammoProuch;
    }
    public void Performed(Weapon weapon)
    {
        Debug.Log("Reload finish");
        BulletType bulletType = weapon.bullet.GetComponent<Bullet>().type;
        int magCount = weapon.Magazine_count;
        int magCapacity = weapon.Magazine_capacity;
        if (ammoProuch.amountOf_ammo[bulletType] > 0)
        {
            int fillamout = magCapacity - magCount;
            if(ammoProuch.amountOf_ammo[bulletType] - fillamout < 0)
            {
                int minusAmmo = ammoProuch.amountOf_ammo[bulletType] -= fillamout;
                ammoProuch.amountOf_ammo[bulletType] = 0;
                weapon.Magazine_count += fillamout + minusAmmo;
            }
            else
            {
                ammoProuch.amountOf_ammo[bulletType] -= fillamout;
                weapon.Magazine_count += fillamout;
            }
           
        }
    }
   
}

  
