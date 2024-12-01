using System.Collections.Generic;
using UnityEngine;

public class WeaponBlackBoard : IObserverWeapon
{
    private Weapon Weapon;
    public TriggerState TriggerState { get => Weapon.triggerState; }
    public Dictionary<BulletStackType, int> BulletStack { get => Weapon.bulletStore; }

    public bool IsAiming { get => Weapon.isAiming; }
    public bool IsReloadCommand { get => Weapon.isReloadCommand; }
    public bool IsEquip { get { return Weapon.userWeapon == null? false : true; } }
    public bool IsCancle { get => Weapon.isCancelAction; }
    public float Rate_of_fire { get => Weapon.rate_of_fire; }
   public WeaponBlackBoard(Weapon weapon) 
   {
        Weapon = weapon;
   }

    public void OnNotify(Weapon weapon, WeaponSubject.WeaponNotifyType weaponNotify)
    {
        
    }
}
