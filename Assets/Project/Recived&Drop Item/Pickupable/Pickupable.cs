using UnityEngine;

/// <summary>
/// World-space item. Holds a <see cref="PickupableDefinition"/> (what the item is)
/// and delegates HOW it gets picked up to a sibling <see cref="IPickupBehavior"/>
/// MonoBehaviour (Magnet / Trigger / future Interact).
///
/// Behaviors call back via <see cref="TryConsume"/> when they detect a valid receiver.
/// </summary>
public class Pickupable : MonoBehaviour
{
    [SerializeField] private PickupableDefinition definition;

    private IPickupBehavior behavior;
    private bool consumed;

    public PickupableDefinition Definition => definition;

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
        if (definition == null || receiver == null) return false;
        var effects = definition.effects;
        if (effects == null) return false;
        for (int i = 0; i < effects.Count; i++)
            if (effects[i] != null && effects[i].CanBeReceivedBy(receiver)) return true;
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

        var effects = definition.effects;
        for (int i = 0; i < effects.Count; i++)
            effects[i]?.Apply(receiver, this);

        consumed = true;
        return true;
    }

    public bool Consumed => consumed;
}
