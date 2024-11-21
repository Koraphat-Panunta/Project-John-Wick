using UnityEngine;

public interface IPlayerComponent:IObserverPlayer
{
    public void UpdateComponent();
    public void FixedUpdateComponent();
}
