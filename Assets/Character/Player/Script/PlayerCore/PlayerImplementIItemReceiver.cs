using UnityEngine;

/// <summary>
/// Player's implementation of the unified IItemReceiver capability.
/// Supersedes the legacy IRecivedAble / IHPReciveAble / IAmmoRecivedAble
/// triplet. Internally still uses the existing AddHP / ammoProuch.AddAmmo /
/// ApplyPowerUp APIs — only the entry points are new.
/// </summary>
public partial class Player : IItemReceiver
{
    Transform IItemReceiver.transform => centreTransform;

    // ---------- Capability queries ----------------------------------------
    public bool HasHpRoom() => GetHP() < GetMaxHp();

    public bool HasAmmoRoom(BulletType type)
    {
        if (_weaponBelt == null || _weaponBelt.ammoProuch == null) return false;
        return _weaponBelt.ammoProuch.CheckAmmo(type) < _weaponBelt.ammoProuch.CheckMaxAmmo(type);
    }

    // ---------- Receive entry points -------------------------------------
    public void ReceiveHp(HpEffect effect)
    {
        if (effect == null) return;

        // Preserve the legacy "low-HP top-up to 35% then add" behaviour from
        // the old IHPReciveAble.Recived method.
        if ((GetHP() / GetMaxHp()) < 0.35f)
        {
            AddHP(Mathf.Abs((this.GetMaxHp() * 0.35f) - GetHP()));
            AddHP(effect.amount);
        }
        else
        {
            AddHP(effect.amount);
        }

        NotifyObserver(this, NotifyEvent.ReceiveItem);
    }

    public void ReceiveAmmo(AmmoEffect effect)
    {
        if (effect == null || _weaponBelt == null || _weaponBelt.ammoProuch == null) return;
        _weaponBelt.ammoProuch.AddAmmo(BulletType.handgunAmmo, effect.handgunAmmo);
        _weaponBelt.ammoProuch.AddAmmo(BulletType.rifleAmmo, effect.rifleAmmo);
        _weaponBelt.ammoProuch.AddAmmo(BulletType.battleRifleAmmo, effect.battleRifleAmmo);
        _weaponBelt.ammoProuch.AddAmmo(BulletType.buckShotAmmo, effect.shotgunAmmo);
        NotifyObserver(this, NotifyEvent.ReceiveItem);
    }

    public void ReceivePowerUp(PowerUpScriptableObject def, object source)
    {
        // Routes through the existing IPowerUpReceiver implementation.
        ApplyPowerUp(def, source);
    }
}
