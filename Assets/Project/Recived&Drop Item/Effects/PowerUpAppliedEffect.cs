using System;
using UnityEngine;

/// <summary>
/// Applies a <see cref="PowerUpScriptableObject"/> when this item is picked up.
/// The Pickupable instance is used as the removal source key.
/// </summary>
[Serializable]
public class PowerUpAppliedEffect : IPickupEffect
{
    [Tooltip("Power-up definition (stat changes, duration). Reused unchanged from the existing PowerUp system.")]
    public PowerUpScriptableObject definition;

    public bool CanBeReceivedBy(IItemReceiver receiver) => definition != null && receiver != null;

    public void Apply(IItemReceiver receiver, object source)
    {
        if (definition == null || receiver == null) return;
        receiver.ReceivePowerUp(definition, source);
    }
}
