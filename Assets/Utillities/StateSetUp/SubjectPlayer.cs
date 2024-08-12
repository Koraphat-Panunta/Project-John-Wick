using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SubjectPlayer : MonoBehaviour
{
    public enum PlayerAction
    {
        SwapShoulder,
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
    protected void NotifyObserver(PlayerController playerController,PlayerAction playerAction)
    {
        foreach (IObserverPlayer observer in Observers)
        {
            observer.OnNotify(playerController,playerAction);
        }
    }

}
