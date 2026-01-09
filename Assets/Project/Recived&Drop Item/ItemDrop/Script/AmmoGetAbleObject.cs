using UnityEngine;

public class AmmoGetAbleObject : ItemObject
{
    [Range(0, 100)]
    [SerializeField] public int amoutAmmoAdd;

    

    protected override void RecivedAbleRecivedItem(IRecivedAble client)
    {

        (client as IAmmoRecivedAble).ammoProuch.AddAmmo(amoutAmmoAdd);
        (client as IAmmoRecivedAble).Recived(this);

        base.RecivedAbleRecivedItem(client);
    }

    protected override void Update()
    {
        base.Update();
    }
}
