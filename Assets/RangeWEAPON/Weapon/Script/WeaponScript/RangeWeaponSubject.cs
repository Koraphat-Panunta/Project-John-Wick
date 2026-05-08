using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class RangeWeaponSubject : MonoBehaviour
{
    List<IObserverRangeWeapon> _observers = new List<IObserverRangeWeapon>();
    public enum WeaponNotifyType
    {
        ReleseMagazine,

        AttachmentSetup,

        BeenAttatch,
        BeenDetatch,
        Rest
    }
    public void Notify<T>(RangeWeapon weapon,T weaponNotifyType)
    {
        for(int i =0; i<= _observers.Count - 1; i++)
        {
            if (_observers[i]!= null)
            {
                _observers[i].OnNotify(weapon, weaponNotifyType);
            }
        }
    }
    public void AddObserver(IObserverRangeWeapon observer)
    {
        _observers.Add(observer);
    }
    public void Remove(IObserverRangeWeapon observer)
    {
        _observers.Remove(observer);
    }
}
