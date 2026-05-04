using UnityEngine;

/// <summary>
/// Single unified receiver capability. Replaces the old IRecivedAble / IHPReciveAble /
/// IAmmoRecivedAble triplet. Each effect type calls the matching Receive* method
/// (visitor-style) so dispatch stays type-safe with no runtime switching.
///
/// Capability queries (Has*Room) let pickups decide whether to attract / be consumed
/// without forcing the receiver to no-op silently.
/// </summary>
public interface IItemReceiver
{
    Transform transform { get; }

    // --- Capability queries (per resource/stat) -------------------------
    bool HasHpRoom();
    bool HasAmmoRoom(BulletType type);

    // --- Receive entry points (one per effect type) ---------------------
    void ReceiveHp(HpEffect effect);
    void ReceiveAmmo(AmmoEffect effect);
    void ReceivePowerUp(PowerUpScriptableObject def, object source);
}
