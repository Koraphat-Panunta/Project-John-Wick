using System.Collections;
using UnityEngine;

/// <summary>
/// World-space power-up. Sit on a GameObject with a trigger collider.
/// On touch by an IPowerUpReceiver (e.g. Player), applies the
/// <see cref="definition"/>'s effects with this pickup as the source key.
/// If the definition has duration > 0, the effects are auto-removed
/// after that many seconds.
/// </summary>
[RequireComponent(typeof(Collider))]
public class PowerUpPickup : MonoBehaviour
{
    [SerializeField] private PowerUpScriptableObject definition;

    [Tooltip("If true, destroys the pickup GameObject after applying. " +
             "Note: with a non-zero duration the timed removal coroutine " +
             "still needs a runner — when destroyed, it switches to a detached runner.")]
    [SerializeField] private bool destroyOnPickup = true;

    private bool consumed;

    private void Reset()
    {
        // Make the collider a trigger by default for convenience.
        var c = GetComponent<Collider>();
        if (c != null) c.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (consumed) return;
        if (definition == null)
        {
            Debug.LogWarning("[PowerUpPickup] No definition assigned.", this);
            return;
        }

        var receiver = other.GetComponentInParent<IPowerUpReceiver>();
        if (receiver == null) return;

        receiver.ApplyPowerUp(definition, this);
        consumed = true;

        if (definition.duration > 0f)
            StartTimedRemoval(receiver, definition.duration);

        if (destroyOnPickup)
        {
            // Hide visuals and disable collider, but keep the GameObject alive
            // long enough for any timed-removal coroutine to finish referencing 'this'.
            foreach (var r in GetComponentsInChildren<Renderer>()) r.enabled = false;
            foreach (var col in GetComponentsInChildren<Collider>()) col.enabled = false;

            float linger = Mathf.Max(definition.duration, 0f) + 0.1f;
            Destroy(gameObject, linger);
        }
    }

    private void StartTimedRemoval(IPowerUpReceiver receiver, float seconds)
    {
        StartCoroutine(RemoveAfter(receiver, seconds));
    }

    private IEnumerator RemoveAfter(IPowerUpReceiver receiver, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        receiver?.RemovePowerUp(this);
    }
}
