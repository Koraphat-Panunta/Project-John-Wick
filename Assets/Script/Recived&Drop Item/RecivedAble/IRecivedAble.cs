using UnityEngine;

public interface IRecivedAble 
{
    public Transform transform { get; }
    public bool PreCondition(ItemObject itemObject);
}
