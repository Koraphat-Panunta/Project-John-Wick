using System.Collections.Generic;
using UnityEngine;

public class WeaponBlackBoard : IObserverWeapon
{
    private Weapon Weapon;
    public TriggerState TriggerState { get => Weapon.triggerState; }
    public Dictionary<BulletStackType, int> BulletStack { get => Weapon.bulletStore; }

    public bool IsAiming { get => Weapon.isAiming; }
    public bool IsReloadCommand { get => Weapon.isReloadCommand; }
    public bool IsEquip { get { return Weapon.userWeapon == null? true : false; } }
    public bool IsCancle { get => Weapon.isCancelAction; }
    public float Rate_of_fire { get => Weapon.rate_of_fire; }

   
    public struct VarAttribute
    {
        public Weapon weapon1 { get; set; }
        public TriggerState trigger { get => weapon1.triggerState; } 
    }
    VarAttribute varAttribute = new VarAttribute();
   public WeaponBlackBoard(Weapon weapon) 
   {
        varAttribute.weapon1 = weapon;
       
       
   }

    public void OnNotify(Weapon weapon, WeaponSubject.WeaponNotifyType weaponNotify)
    {
       
    }
}
