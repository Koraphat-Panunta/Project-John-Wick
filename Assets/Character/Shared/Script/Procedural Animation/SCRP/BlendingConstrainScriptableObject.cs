using UnityEngine;


public interface BlendingConstrainScriptableObject<T> where T : ScriptableObject
{
    public BledningWeightSCRP<T>[] _bledningWeightSCRP { get; }
}
[System.Serializable]
public struct BledningWeightSCRP<T> where T : ScriptableObject
{
    public float weight;
    public T scrp;
}
