
using System.Collections.Generic;

public abstract class SubjectPlayer : Character
{
    public enum PlayerAction
    {
        SwapShoulder,
        Aim,
        AimHumandShield,
        LowReady,
        Firing,
        Reloading,
        SwitchWeapon,
        PickUpWeapon,

        Idle,
        Move,
        Sprint,
        Dodge,

        GetShoot,
        HealthRegen,
        Dead,
       
        TakeCover,
        GetOffCover,

        GunFuEnter,
        GunFuExit
        
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
    public void NotifyObserver(Player player,PlayerAction playerAction)
    {
        foreach (IObserverPlayer observer in this.observers)
        {
            if (observer == null)
                this.observers.Remove(observer);
            else
                observer.OnNotify(player,playerAction);
        }
    }
    public void NotifyObserver(Player player)
    {
        foreach (IObserverPlayer observer in this.observers)
        {
            if (observer == null)
                this.observers.Remove(observer);
            else
                observer.OnNotify(player);
        }
    }

}
