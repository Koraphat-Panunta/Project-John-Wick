using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SubjectEnemy : Character
{
    public enum EnemyEvent
    {
        GotBulletHit,
        HeardingGunShoot,
        OnEnable,
        OnDisable,
    } 
    protected List<IObserverEnemy> Observers = new List<IObserverEnemy>();
    public void AddObserver(IObserverEnemy observer)
    {
        if(Observers.Contains(observer) == false)
            Observers.Add(observer);
    }
    public void RemoveObserver(IObserverEnemy observer) 
    {
        Observers.Remove(observer);
    }
  
    public void NotifyObserver<T>(Enemy enemy,T node)
    {

        if (Observers.Count > 0)
        {
            for (int i = Observers.Count - 1; i >= 0; i--)
            {
                if (Observers[i] == null)
                {
                    Observers.RemoveAt(i);
                }
                else
                {
                    Observers[i].Notify(enemy, node);
                }

            }
        }
    }

}
