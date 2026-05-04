using System;
using UnityEngine;

[Serializable]
public class HpEffect : IPickupEffect
{
    [Range(0, 200)] public float amount = 20f;

    public bool CanBeReceivedBy(IItemReceiver receiver) => receiver != null && receiver.HasHpRoom();

    public void Apply(IItemReceiver receiver, object source) => receiver?.ReceiveHp(this);
}
