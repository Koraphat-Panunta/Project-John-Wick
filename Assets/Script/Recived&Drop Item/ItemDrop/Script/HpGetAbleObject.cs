using UnityEngine;

public class HpGetAbleObject : ItemObject
{
    [Range(0, 100)]
    [SerializeField] private float amoutOfHpAdd;

    protected override void SetVisitorClient(IRecivedAble client)
    {
        (client as IHPReciveAble).character.AddHP(amoutOfHpAdd);
        (client as IHPReciveAble).Recived(this);
    }
}
