using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SubjectPlayer : Character
{
    public enum PlayerAction
    {
        SwapShoulder,
        Aim,
        LowReady,
        Firing,
        Reloading,
        Idle,
        Move,
        Sprint,
        GetWeapon
    }
    private List<IObserverPlayer> Observers = new List<IObserverPlayer>();
    public void AddObserver(IObserverPlayer observer)
    {
        this.Observers.Add(observer);
    }
    public void RemoveObserver(IObserverPlayer observer)
    {
        this.Observers.Remove(observer);
    }
    public void NotifyObserver(Player player,PlayerAction playerAction)
    {
        foreach (IObserverPlayer observer in Observers)
        {
            observer.OnNotify(player,playerAction);
        }
    }

}
