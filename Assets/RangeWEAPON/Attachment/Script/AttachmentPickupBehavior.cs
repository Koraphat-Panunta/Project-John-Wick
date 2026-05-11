using UnityEngine;

/// <summary>
/// Trigger-based pickup behavior for world-space attachment items.
/// After the WeaponAttachment (IPickupItem) re-parents itself onto the
/// weapon socket, this behavior strips the pickup infrastructure
/// (Collider, Rigidbody, Pickupable, itself) so the attachment remains
/// alive but no longer acts as a world item.
/// </summary>
[RequireComponent(typeof(Pickupable))]
[RequireComponent(typeof(WeaponAttachment))]
public class AttachmentPickupBehavior : MonoBehaviour, IPickupBehavior
{
    private Pickupable owner;

    public void Initialize(Pickupable o)
    {
        owner = o;
        foreach (var c in GetComponents<Collider>())
            c.isTrigger = true;
    }

    public void OnConsumed()
    {
        var rb = GetComponent<Rigidbody>();
        if (rb != null) Destroy(rb);
        foreach (var col in GetComponents<Collider>()) Destroy(col);
        if (owner != null) Destroy(owner);
        Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner == null || owner.Consumed) return;
        var receiver = other.GetComponentInParent<IItemReceiver>();
        if (receiver == null) return;
        if (owner.TryConsume(receiver))
            OnConsumed();
    }
}
