using UnityEngine;

public class AmmoGetAbleObject : ItemObject
{
    [SerializeField] private int handgunAmmo = 5;
    [SerializeField] private int rifleAmmo = 3;
    [SerializeField] private int battleRifleAmmo = 1;
    [SerializeField] private int shotgunAmmo = 1;

    protected override void RecivedAbleRecivedItem(IRecivedAble client)
    {

        (client as IAmmoRecivedAble).ammoProuch.AddAmmo(BulletType.handgunAmmo,handgunAmmo);
        (client as IAmmoRecivedAble).ammoProuch.AddAmmo(BulletType.rifleAmmo,rifleAmmo);
        (client as IAmmoRecivedAble).ammoProuch.AddAmmo(BulletType.battleRifleAmmo,battleRifleAmmo);
        (client as IAmmoRecivedAble).ammoProuch.AddAmmo(BulletType.buckShotAmmo,shotgunAmmo);

        (client as IAmmoRecivedAble).Recived(this);

        base.RecivedAbleRecivedItem(client);
    }

    protected override void Update()
    {
        base.Update();
    }
}
