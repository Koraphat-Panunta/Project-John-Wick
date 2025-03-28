using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponFactory 
{
    public Weapon prefabWeapon;
    public abstract void CreateWeapon(IWeaponAdvanceUser weaponUser);
    public abstract Weapon GetCreateWeapon(IWeaponAdvanceUser weaponUser);


}
