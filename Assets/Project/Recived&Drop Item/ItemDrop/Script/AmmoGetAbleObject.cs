using UnityEngine;

public class AmmoGetAbleObject : ItemObject
{
    [Range(0, 100)]
    [SerializeField] public int amoutAmmoAdd;

    

    protected override void SetVisitorClient(IRecivedAble client)
    {

        (client as IAmmoRecivedAble).ammoProuch.AddAmmo(amoutAmmoAdd);
        (client as IAmmoRecivedAble).Recived(this);
    }

    protected override void Update()
    {
        base.Update();
    }
}
