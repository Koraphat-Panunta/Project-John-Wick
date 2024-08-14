using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponSenses 
{
    public void Aiming(Weapon weapon);
    public void Reloading(Weapon weapon,Reload.ReloadType reloadType);
    public void Firing(Weapon weapon);
    public void LowReadying(Weapon weapon);
}
