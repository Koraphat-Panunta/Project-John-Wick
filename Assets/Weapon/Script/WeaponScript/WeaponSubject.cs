using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSubject : MonoBehaviour
{
    List<IObserverWeapon> _observers = new List<IObserverWeapon>();
    public enum WeaponNotifyType
    {
        Reloading,
        TacticalReload,
        Firing,
        AttachmentSetup
    }
    public void Notify(Weapon weapon,WeaponNotifyType weaponNotifyType)
    {
        for(int i =0; i<= _observers.Count - 1; i++)
        {
            if (_observers[i]!= null)
            {
                _observers[i].OnNotify(weapon, weaponNotifyType);
            }
        }
    }
    public void AddObserver(IObserverWeapon observer)
    {
        _observers.Add(observer);
    }
    public void Remove(IObserverWeapon observer)
    {
        _observers.Remove(observer);
    }
}
