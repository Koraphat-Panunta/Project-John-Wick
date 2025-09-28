using UnityEngine;

public interface IObserverTimeControlManager 
{
    public void OnNotifyObserver<T>(TimeControlManager timeControlManager, T Var);
}
