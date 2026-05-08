using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverRangeWeapon 
{
    public abstract void OnNotify<T>(RangeWeapon weapon, T weaponNotify);
   
}
