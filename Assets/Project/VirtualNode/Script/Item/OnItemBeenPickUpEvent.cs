using UnityEngine;

/// <summary>
/// Level virtual node that fires when a watched <see cref="Pickupable"/> gets consumed.
/// Used to chain item pickup → cinematic / mission-progress / next-spawn / etc.
/// </summary>
public class OnItemBeenPickUpEvent : VirtualEventNode
{
    [SerializeField] Pickupable pickupable;

    private void Awake()
    {
        if (pickupable != null)
            pickupable.OnConsumedEvent += HandleConsumed;
    }

    private void OnDestroy()
    {
        if (pickupable != null)
            pickupable.OnConsumedEvent -= HandleConsumed;
    }

    private void HandleConsumed(Pickupable item, IItemReceiver receiver)
    {
        Execute();
        if (pickupable != null)
            pickupable.OnConsumedEvent -= HandleConsumed;
    }

    protected override void OnDrawGizmos()
    {
        if (isEnableGizmos && pickupable != null)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(transform.position, pickupable.transform.position);
        }
        base.OnDrawGizmos();
    }
}
