/// <summary>
/// Strategy that decides HOW a Pickupable detects its receiver and decides
/// to be consumed. Three concrete strategies ship: Magnet (auto-pull),
/// Trigger (collider OnTriggerEnter), and (future) Interact (button press).
///
/// The behavior is a sibling MonoBehaviour on the same Pickupable GameObject
/// so it can hold its own serialized state in the Inspector.
/// </summary>
public interface IPickupBehavior
{
    void Initialize(Pickupable owner);

    /// <summary>
    /// Called by Pickupable when this behavior decides to consume the item.
    /// The behavior is responsible for calling owner.Consume(receiver).
    /// </summary>
    void OnConsumed();
}
