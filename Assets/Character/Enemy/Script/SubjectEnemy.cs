using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectEnemy : Character
{
    public enum EnemyEvent
    {
        //GotHit,
        GotBulletHit,

        //GunFuGotHit,
        //GunFuGotInteract,

        //GunFuEnter,
        //GunFuAttack,
        //GunFuExit,

        //HeardingGunShoot,

        //Dead,

        //Idle,
        //Move,
        //Sprint,

        //FallDown,
        //GetUp,

        //Flanking,
        //TakeCover,
        //TakeAim,
        //Holding,
        //Searching,
        //WarpingMotion,

        //ReloadMagazineFullStage,
        //TacticalReloadMagazineFullStage,
        //SwitchWeapon

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
        }
    }
    public void NotifyObserver<T>(Enemy enemy,T node)where T : INode
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
