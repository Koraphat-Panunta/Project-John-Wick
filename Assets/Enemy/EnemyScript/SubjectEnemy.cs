using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectEnemy : Character
{
    public enum EnemyEvent
    {
        GetShoot_Arm,
        GetShoot_Chest,
        GetShoot_Head,
        GetShoot_Leg,
        Dead,
        Flanking,
        TakeCover,
        Holding,
        Searching,
        WarpingMotion
    } 
    protected List<IObserverEnemy> Observers = new List<IObserverEnemy>();
    public void AddObserver(IObserverEnemy observer)
    {
        Observers.Add(observer);
    }
    public void RemoveObserver(IObserverEnemy observer) 
    {
        Observers.Remove(observer);
    }
    public void NotifyObserver(Enemy enemy,EnemyEvent enemyEvent)
    {
        
        if (Observers.Count > 0)
        {
            for(int i = Observers.Count - 1; i >= 0; i--)
            {
                if (Observers[i] == null)
                {
                    Observers.RemoveAt(i);
                }
                else
                {
                    Observers[i].Notify(enemy, enemyEvent);
                }
                
            }
            //foreach (IObserverEnemy observer in Observers)
            //{
            //    if()
            //    observer.Notify(_enemy, enemyEvent);
            //}
        }
    }
}