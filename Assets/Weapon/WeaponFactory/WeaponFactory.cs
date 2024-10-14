using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponFactory : MonoBehaviour
{
    public Weapon prefabWeapon;
    public abstract void CreateWeapon(Character weaponUser);
    
}
