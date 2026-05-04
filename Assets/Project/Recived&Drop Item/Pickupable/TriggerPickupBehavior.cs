using System.Collections;
using UnityEngine;

/// <summary>
/// Trigger-collider pickup. Walks-into-it: as soon as an IItemReceiver enters the
/// trigger AND wants the item, it's consumed. Best for power-ups, key items, or
/// anything that should NOT pull toward the player.
///
/// If the underlying definition includes a PowerUpAppliedEffect with duration > 0,
/// the GameObject lingers (visuals + colliders disabled) so it stays valid as the
/// removal source key for the timed coroutine.
/// </summary>
[RequireComponent(typeof(Pickupable))]
[RequireComponent(typeof(Collider))]
public class TriggerPickupBehavior : MonoBehaviour, IPickupBehavior
{
    private Pickupable owner;
    private float lingerSeconds; // computed from any PowerUp effect's duration

    public void Initialize(Pickupable o)
    {
        owner = o;
        var c = GetComponent<Collider>();
        if (c != null && !c.isTrigger) c.isTrigger = true;
        ComputeLingerFromDefinition();
    }

    public void OnConsumed()
    {
        // Hide visuals + disable colliders, then destroy after linger so timed
        // power-ups can still reference 'this' as the removal source.
        foreach (var r in GetComponentsInChildren<Renderer>()) r.enabled = false;
        foreach (var col in GetComponentsInChildren<Collider>()) col.enabled = false;
        Destroy(gameObject, lingerSeconds + 0.1f);
    }

    private void ComputeLingerFromDefinition()
    {
        lingerSeconds = 0f;
        if (owner == null || owner.Definition == null || owner.Definition.effects == null) return;
        foreach (var e in owner.Definition.effects)
        {
            if (e is PowerUpAppliedEffect pwr && pwr.definition != null && pwr.definition.duration > lingerSeconds)
                lingerSeconds = pwr.definition.duration;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner == null || owner.Consumed) return;

        var receiver = other.GetComponentInParent<IItemReceiver>();
        if (receiver == null) return;

        if (owner.TryConsume(receiver))
        {
            // After consuming, schedule timed-removal of any duration-based power-up.
            ScheduleTimedRemoval(receiver);
            OnConsumed();
        }
    }

    private void ScheduleTimedRemoval(IItemReceiver receiver)
    {
        if (owner == null || owner.Definition == null || owner.Definition.effects == null) return;
        foreach (var e in owner.Definition.effects)
        {
            if (e is PowerUpAppliedEffect pwr && pwr.definition != null && pwr.definition.duration > 0f)
                StartCoroutine(RemoveAfter(receiver, pwr.definition.duration, owner));
        }
    }

    private IEnumerator RemoveAfter(IItemReceiver receiver, float seconds, object source)
    {
        yield return new WaitForSeconds(seconds);
        // Player implements IPowerUpReceiver; route through that interface to remove.
        if (receiver is IPowerUpReceiver pr) pr.RemovePowerUp(source);
    }
}
