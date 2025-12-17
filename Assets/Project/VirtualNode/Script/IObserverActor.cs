using UnityEngine;

public interface IObserverActor 
{
    public void OnNotifyActor<T>(Actor actor, T var);
   
}
