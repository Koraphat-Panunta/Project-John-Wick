using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;


public class WeaponActionEvent 
{
    //public enum WeaponEvent
    //{
    //    Fire,
    //    Aim,
    //    LowReady,
    //    Reload,
    //}
    //public event Action<Weapon> Event;
    //private static readonly IDictionary <WeaponEvent, Action<Weapon>> EventDictionary = new Dictionary<WeaponEvent, Action<Weapon> >();
    //public static void Scubscibtion(WeaponEvent weaponEvent, Action<Weapon> Event )
    //{
    //    Action<Weapon> Events;
    //    if (EventDictionary.TryGetValue(weaponEvent,out Events))
    //    {
    //        EventDictionary[weaponEvent] += Event;
    //    }
    //    else
    //    {
    //        EventDictionary.Add(weaponEvent, Events);
    //        EventDictionary[weaponEvent] += Event;
    //    }
    //    //UnityEngine.Debug.Log("Event Sub :" + Event.Method.Name);
       
    //}
    //public static void UnSubscirbe(WeaponEvent weaponEvent,Action<Weapon> Event)
    //{
    //    EventDictionary[weaponEvent] -= Event;
    //}
    //public static void Publish(WeaponEvent weaponEvent,Weapon weapon)
    //{
      
    //    if (EventDictionary.TryGetValue(weaponEvent ,out var Event))
    //    {
    //        if (EventDictionary[weaponEvent] != null)
    //        {
    //            EventDictionary[weaponEvent].Invoke(weapon);
    //        }
    //    }
       
    //}
    
}
