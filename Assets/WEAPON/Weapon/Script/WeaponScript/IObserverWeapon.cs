using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverWeapon 
{
    public abstract void OnNotify<T>(Weapon weapon, T weaponNotify);
   
}
