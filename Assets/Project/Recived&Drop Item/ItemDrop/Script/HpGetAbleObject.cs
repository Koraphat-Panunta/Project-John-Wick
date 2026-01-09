using UnityEngine;

public class HpGetAbleObject : ItemObject
{
    [Range(0, 100)]
    [SerializeField] public float amoutOfHpAdd;

    protected override void RecivedAbleRecivedItem(IRecivedAble client)
    {
        (client as IHPReciveAble).Recived(this);
        base.RecivedAbleRecivedItem(client);
    }
}
