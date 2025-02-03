using UnityEngine;

public class HpGetAbleObject : ItemObject<IHPReciveAble>
{
    [Range(0, 100)]
    [SerializeField] private float amoutOfHpAdd;
    protected override void SetVisitorClient(IHPReciveAble client)
    {
        client.character.AddHP(amoutOfHpAdd);
        client.Recived(this);
    }
}
