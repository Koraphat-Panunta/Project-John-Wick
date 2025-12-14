using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static TriggerBox;

public abstract class Actor : MonoBehaviour
{
    [SerializeField] List<IObserverActor> observer = new List<IObserverActor>();

    public void AddTriggerBoxObserver(IObserverActor observerTriggerBox)
    {
        this.observer.Add(observerTriggerBox);
    }
    public void RemoveTriggerBoxObserver(IObserverActor observerTriggerBox)
    {
        this.observer.Remove(observerTriggerBox);
    }
    protected void NotifyObserver<T>(T var)
    {
        if (this.observer.Count <= 0)
            return;

        for (int i = 0; i < this.observer.Count; i++)
        {
            this.observer[i].OnNotifyActor(this, var);
        }
    }
    [SerializeField] protected bool isEnableGizmos;
    protected virtual void OnDrawGizmos()
    {
        if(this.isEnableGizmos == false)
            return;

        Handles.Label(transform.position, this.name);
    }
}
