using UnityEngine;


public abstract class BlendingConstrainScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    public BledningWeightSCRP<T>[] bledningWeightSCRP;
}
[System.Serializable]
public struct BledningWeightSCRP<T> where T : ScriptableObject
{
    public float weight;
    public T scrp;
}
