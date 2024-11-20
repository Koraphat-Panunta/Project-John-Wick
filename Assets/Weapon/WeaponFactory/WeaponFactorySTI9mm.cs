using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactorySTI9mm : WeaponFactory
{

    public override void CreateWeapon(IWeaponAdvanceUser weaponUser)
    {
        prefabWeapon = Resources.Load<Weapon>("STI_9mm");
        
        Weapon Weapon = GameObject.Instantiate(prefabWeapon,weaponUser.weaponUserAnimator.transform.position,Quaternion.identity);
        Weapon.AttatchWeaponTo(weaponUser);
    }
    public override Weapon GetCreateWeapon(IWeaponAdvanceUser weaponUser)
    {
        prefabWeapon = Resources.Load<Weapon>("STI_9mm");
        Weapon Weapon = GameObject.Instantiate(prefabWeapon, weaponUser.weaponUserAnimator.transform.position, Quaternion.identity);
        Weapon.AttatchWeaponTo(weaponUser);
        return Weapon;
    }
}
