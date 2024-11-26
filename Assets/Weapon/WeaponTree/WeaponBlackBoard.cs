using System.Collections.Generic;
using UnityEngine;

public class WeaponBlackBoard : IObserverWeapon
{
    private Weapon Weapon;
    public TriggerState TriggerState { get => Weapon.triggerState; }
    public Dictionary<BulletStackType, int> BulletStack { get => Weapon.bulletStore; }
    public bool IsAiming { get => Weapon.isAiming; }
    public bool IsReloading { get => Weapon.isReloading; }
   public WeaponBlackBoard(Weapon weapon) 
   {
        Weapon = weapon;
   }

    public void OnNotify(Weapon weapon, WeaponSubject.WeaponNotifyType weaponNotify)
    {
        
    }
}
