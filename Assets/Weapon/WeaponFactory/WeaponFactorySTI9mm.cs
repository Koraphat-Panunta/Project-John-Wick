using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class WeaponFactorySTI9mm : WeaponFactory
{

    public override void CreateWeapon(Character weaponUser)
    {
        prefabWeapon = Resources.Load<Weapon>("STI_9mm");
        Weapon Weapon = GameObject.Instantiate(prefabWeapon,weaponUser.transform.position,Quaternion.identity);
        Weapon.AttatchWeaponTo(weaponUser);
    }
}
