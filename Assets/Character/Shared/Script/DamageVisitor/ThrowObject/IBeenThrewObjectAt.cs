using UnityEngine;

public interface IBeenThrewObjectAt : IDamageAble
{
    public Vector3 _beenThrowObjectAtPosition { get; set; }
}
