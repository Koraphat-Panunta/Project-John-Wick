using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{
    private List<IObserver> Observers = new List<IObserver>();
    public void AddObserver(IObserver observer)
    {
        this.Observers.Add(observer);
    }
    public void RemoveObserver(IObserver observer)
    {
        this.Observers.Remove(observer);
    }
    protected void NotifyObserver()
    {
        foreach (IObserver observer in Observers)
        {
            observer.OnNotify();
        }
    }

}
