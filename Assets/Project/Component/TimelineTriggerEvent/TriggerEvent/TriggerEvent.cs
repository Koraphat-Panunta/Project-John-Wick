using UnityEngine;

[System.Serializable]
public abstract class TriggerEvent<T> 
{
    [Range(0f, 1f)]
    public float normalizedTime;

    public abstract void Initilaized(T var);

    public abstract void Execute();
   
}
