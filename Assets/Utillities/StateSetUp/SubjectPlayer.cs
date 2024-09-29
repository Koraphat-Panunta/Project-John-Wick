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
        PickUpWeapon,
        GetShoot,
        HealthRegen,
        Dead
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
        //foreach (IObserverPlayer observer in Observers)
        //{
        //    observer.OnNotify(player,playerAction);
        //}
        for (int i = Observers.Count - 1; i >= 0; i--)
        {
            if (Observers[i] == null)
            {
                Observers.RemoveAt(i);
            }
            else
            {
                Observers[i].OnNotify(player, playerAction);
            }
        }
    }

}
