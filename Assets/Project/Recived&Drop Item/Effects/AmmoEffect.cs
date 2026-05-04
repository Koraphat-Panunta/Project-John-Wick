using System;
using UnityEngine;

[Serializable]
public class AmmoEffect : IPickupEffect
{
    [Range(0, 200)] public int handgunAmmo = 5;
    [Range(0, 200)] public int rifleAmmo = 3;
    [Range(0, 200)] public int battleRifleAmmo = 1;
    [Range(0, 200)] public int shotgunAmmo = 1;

    public bool CanBeReceivedBy(IItemReceiver receiver)
    {
        if (receiver == null) return false;
        return (handgunAmmo > 0 && receiver.HasAmmoRoom(BulletType.handgunAmmo))
            || (rifleAmmo > 0 && receiver.HasAmmoRoom(BulletType.rifleAmmo))
            || (battleRifleAmmo > 0 && receiver.HasAmmoRoom(BulletType.battleRifleAmmo))
            || (shotgunAmmo > 0 && receiver.HasAmmoRoom(BulletType.buckShotAmmo));
    }

    public void Apply(IItemReceiver receiver, object source) => receiver?.ReceiveAmmo(this);
}
