using UnityEngine;

public interface IUpdateAble
{
    public int _updatedOrder { get; }
    public bool isActive { get; }
    public void UpdateOperator();
}
public interface IFixedUpdateAble
{
    public int _fixedUpdateOrder { get; }
    public bool isActive { get; }
    public void FixedUpdateOperator();
   
}
public interface ILateUpdateAble
{
    public int _latedUpdateOrder { get; }
    public bool isActive { get; }
    public void LateUpdateOperator();
}
