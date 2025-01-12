using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverWeapon 
{
    public abstract void OnNotify(Weapon weapon, WeaponSubject.WeaponNotifyType weaponNotify);
   
}
