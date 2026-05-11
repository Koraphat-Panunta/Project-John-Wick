using System;
using UnityEngine;

/// <summary>
/// World-space item. Holds a <see cref="PickupableDefinition"/> (what the item is)
/// and delegates HOW it gets picked up to a sibling <see cref="IPickupBehavior"/>
/// MonoBehaviour (Magnet / Trigger / future Interact).
///
/// Behaviors call back via <see cref="TryConsume"/> when they detect a valid receiver.
/// Subscribers can observe pickup via <see cref="OnConsumedEvent"/> (used by
/// in-world UI badges and level-trigger virtual nodes).
/// </summary>
public class Pickupable : MonoBehaviour
{
    [SerializeField] private PickupableDefinition definition;

    private IPickupBehavior behavior;
    private bool consumed;

    public PickupableDefinition Definition => definition;

    /// <summary>Fired immediately after every effect has been applied to the receiver.</summary>
    public event Action<Pickupable, IItemReceiver> OnConsumedEvent;

    private void Awake()
    {
        behavior = GetComponent<IPickupBehavior>();
        if (behavior == null)
            Debug.LogWarning($"[Pickupable] '{name}' has no IPickupBehavior sibling — it will never be picked up.", this);
        else
            behavior.Initialize(this);
    }

    /// <summary>True if at least one effect can be received by the candidate.</summary>
    public bool CanBeReceivedBy(IItemReceiver receiver)
    {
        if (receiver == null) return false;
        if (definition != null && definition.effects != null)
            for (int i = 0; i < definition.effects.Count; i++)
                if (definition.effects[i] != null && definition.effects[i].CanBeReceivedBy(receiver)) return true;
        foreach (var e in GetComponents<IPickupItem>())
            if (e != null && e.CanBeReceivedBy(receiver)) return true;
        return false;
    }

    /// <summary>
    /// Behavior calls this when it has selected a receiver. Returns true if consumed.
    /// Default disposal: GameObject.Destroy. Override by listening to the OnConsumed
    /// event on the behavior if a different lifecycle is needed.
    /// </summary>
    public bool TryConsume(IItemReceiver receiver)
    {
        if (consumed) return false;
        if (!CanBeReceivedBy(receiver)) return false;

        if (definition != null && definition.effects != null)
            for (int i = 0; i < definition.effects.Count; i++)
                definition.effects[i]?.Apply(receiver, this);

        foreach (var e in GetComponents<IPickupItem>())
            e?.Apply(receiver, this);

        consumed = true;
        OnConsumedEvent?.Invoke(this, receiver);
        return true;
    }

    public bool Consumed => consumed;
}
