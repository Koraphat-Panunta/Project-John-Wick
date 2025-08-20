
using System.Collections.Generic;
using UnityEngine;

public abstract class SubjectPlayer : Character
{
    public enum NotifyEvent
    {
        SwapShoulder,
        //Aim,
        //AimHumandShield,
        //LowReady,
        Firing,
        //SwitchWeapon,
        //QuickDraw,
        //PickUpWeapon,
        //Resting,

        //ReloadMagazineFullStage,
        //TacticalReloadMagazineFullStage,
        //InputMag_ReloadMagazineStage,
        //ChamberLoad_ReloadMagazineStage,

        //StandIdle,
        //StandMove,
        //CrouchIdle,
        //CrouchMove,
        //GetUp,
        //Sprint,
        //Dodge,

        GetShoot,
        GetDamaged,
        HealthRegen,
        TriggerIframe,
        //Dead,

        //TakeCover,
        //GetOffCover,

        //GotAttackGunFuEnter,
        //GotAttackGunFuAttack,
        //GunAttackGunFuExit,

        //GunFuEnter,
        //GunFuInteract,
        //GunFuAttack,
        //GunFuExit,

        OppenentStagger,
        OpponentKilled,

        RecivedAmmo,
        RecivedHp,

    }
    
    protected override void Start()
    {
        base.Start();
    }


    private List<IObserverPlayer> observers = new List<IObserverPlayer>();
    //private List<IObserverPlayer> Observers = new List<IObserverPlayer>();
    public void AddObserver(IObserverPlayer observer)
    {
        observers.Add(observer);
    }
    public void RemoveObserver(IObserverPlayer observer)
    {
        if(observers.Contains(observer))
            observers.Remove(observer);
    }
    public void NotifyObserver<T>(Player player,T node)
    {
        foreach (IObserverPlayer observer in this.observers)
        {
            if (observer == null)
            {
                this.observers.Remove(observer);
            }
            else
            {
                observer.OnNotify(player, node);
            }
        }
    }

}
