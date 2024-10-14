using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactorySTI9mm : WeaponFactory
{
    public override void CreateWeapon(Character weaponUser)
    {
        Weapon Weapon = GameObject.Instantiate(prefabWeapon);
        Weapon.AttatchWeaponTo(weaponUser);
    }
}
