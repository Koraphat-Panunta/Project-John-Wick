using UnityEngine;

public class MuzzleFlashScript : MonoBehaviour, IObserverWeapon
{
    [SerializeField] private GunMuzzleTest gunMuzzleTest;
    public void OnNotify<T>(Weapon weapon, T weaponNotify)
    {
        if(weaponNotify is WeaponSubject.WeaponNotifyType weaponNotifyMassage
            && weaponNotifyMassage == WeaponSubject.WeaponNotifyType.Firing)
            gunMuzzleTest.Fire();
    }

   
}
