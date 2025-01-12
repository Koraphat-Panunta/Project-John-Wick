using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactoryAR15 : WeaponFactory
{
    public override void CreateWeapon(IWeaponAdvanceUser weaponUser)
    {
        prefabWeapon = Resources.Load<Weapon>("AR15_556");
        Weapon Weapon = GameObject.Instantiate(prefabWeapon, weaponUser.weaponUserAnimator.transform.position, Quaternion.identity);
        Weapon.AttatchWeaponTo(weaponUser);
    }
    public override Weapon GetCreateWeapon(IWeaponAdvanceUser weaponUser)
    {
        prefabWeapon = Resources.Load<Weapon>("AR15_556");
        Weapon Weapon = GameObject.Instantiate(prefabWeapon, weaponUser.weaponUserAnimator.transform.position, Quaternion.identity);
        Weapon.AttatchWeaponTo(weaponUser);
        return Weapon;
    }

}
