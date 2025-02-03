using UnityEngine;

public interface IDropAble
{
    public Transform transform { get; set; }
    public void Drop();
}
