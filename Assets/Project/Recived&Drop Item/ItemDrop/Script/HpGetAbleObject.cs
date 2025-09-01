using UnityEngine;

public class HpGetAbleObject : ItemObject
{
    [Range(0, 100)]
    [SerializeField] public float amoutOfHpAdd;

    protected override void SetVisitorClient(IRecivedAble client)
    {
        (client as IHPReciveAble).Recived(this);
    }
}
