using UnityEngine;

public class MuzzleFlashScript : MonoBehaviour, IObserverWeapon
{
    [SerializeField] private GunMuzzleTest gunMuzzleTest;
    public void OnNotify(Weapon weapon, WeaponSubject.WeaponNotifyType weaponNotify)
    {
        if(weaponNotify == WeaponSubject.WeaponNotifyType.Firing)
            gunMuzzleTest.Fire();
    }

   
}
