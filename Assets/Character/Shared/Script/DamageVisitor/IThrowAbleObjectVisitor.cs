using UnityEngine;

public interface IThrowAbleObjectVisitor 
{
    public Vector3 velocity { get; set; }
    public Vector3 position { get; set; }
}
