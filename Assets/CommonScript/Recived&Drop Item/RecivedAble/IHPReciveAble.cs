using UnityEngine;

public interface IHPReciveAble : IRecivedAble
{
   Character character { get; set; }
    public void Recived(HpGetAbleObject hpGetAbleObject);
}
