/// <summary>
/// One stateless action that a <see cref="Pickupable"/> can deliver to an <see cref="IItemReceiver"/>.
/// Implementations are serialized into <see cref="PickupableDefinition.effects"/> via
/// Unity's [SerializeReference] (so the list can hold mixed concrete types).
///
/// Lifecycle:
///   1. Pickupable polls <see cref="CanBeReceivedBy"/> per effect to decide if the receiver
///      wants any part of the item (returns true if ANY effect can be received).
///   2. On pickup, Pickupable invokes <see cref="Apply"/> on every effect.
///
/// The <c>source</c> argument is the Pickupable instance itself — used for removable
/// effects (e.g. timed power-ups) so they can be undone later by the same key.
/// </summary>
public interface IPickupEffect
{
    bool CanBeReceivedBy(IItemReceiver receiver);
    void Apply(IItemReceiver receiver, object source);
}
