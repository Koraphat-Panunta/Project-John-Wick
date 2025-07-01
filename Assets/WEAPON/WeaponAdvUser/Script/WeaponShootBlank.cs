using UnityEngine;

public static class WeaponShootBlank 
{
    public static void ShootBlank(Weapon weapon)
    {
        weapon.Notify(weapon, WeaponSubject.WeaponNotifyType.Firing);
    }
}
