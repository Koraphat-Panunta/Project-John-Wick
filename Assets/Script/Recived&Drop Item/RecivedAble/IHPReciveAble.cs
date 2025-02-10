using UnityEngine;

public interface IHPReciveAble : IRecivedAble
{
   Character character { get; }
    public void Recived(HpGetAbleObject hpGetAbleObject);
}
