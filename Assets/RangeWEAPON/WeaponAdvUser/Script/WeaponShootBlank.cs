using UnityEngine;

public static class WeaponShootBlank 
{
    public static void ShootBlank(RangeWeapon weapon)
    {
        weapon.fire.SetWeaponNodeLeafPhase(WeaponLeafNode.WeaponNodeLeafPhase.Enter);
        weapon.Notify<FiringNode>(weapon, weapon.fire);
    }
}
