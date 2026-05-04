using UnityEngine;

/// <summary>
/// Auto-magnet pickup. Detection uses a trigger SphereCollider (auto-added if absent)
/// instead of per-frame Physics.OverlapSphere — Unity's broadphase handles culling, so
/// no work runs unless something actually enters this pickup's zone.
///
/// Once a valid IItemReceiver is locked, the same physics-pull math as before applies:
/// AddForce toward the receiver, capped by maxPullSpeed, consumed at snapDistance.
///
/// Inspector fields are unchanged from the previous OverlapSphere version, so existing
/// prefab setups keep their tuning.
/// </summary>
[RequireComponent(typeof(Pickupable))]
[RequireComponent(typeof(Rigidbody))]
public class MagnetPickupBehavior : MonoBehaviour, IPickupBehavior
{
    [Range(0, 10)] [SerializeField] private float detectRange = 3f;
    [Range(0, 100)] [SerializeField] private float pullStrength = 8f;
    [Range(0, 100)] [SerializeField] private float maxPullSpeed = 12f;
    [SerializeField] private float snapDistance = 0.45f;
    [Tooltip("Brief warm-up after lock-on before physics pull begins (lets the spawn impulse settle).")]
    [SerializeField] private float armDelay = 0.1f;
    [Tooltip("The non-trigger physics collider (the one that lets the pickup fall and bounce). " +
             "Auto-found at Awake if left empty — picks the first non-trigger Collider on this GameObject.")]
    [SerializeField] private Collider mainCollider;

    private Pickupable owner;
    private Rigidbody rb;
    private SphereCollider triggerZone;

    private IItemReceiver inside;          // currently inside the trigger zone (may not yet qualify)
    private IItemReceiver lockedReceiver;  // qualified receiver we're actively pulling toward
    private bool isPulling;
    private float armTimer;

    public void Initialize(Pickupable o) { owner = o; }
    public void OnConsumed() { Destroy(gameObject); }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (mainCollider == null)
        {
            foreach (var c in GetComponents<Collider>())
            {
                if (!c.isTrigger) { mainCollider = c; break; }
            }
        }
        EnsureTriggerZone();
    }

    private void OnEnable()
    {
        armTimer = 0f;
        isPulling = false;
        lockedReceiver = null;
        inside = null;
    }

    private void OnValidate()
    {
        // Keep the trigger radius in sync if the user tweaks detectRange in Play mode.
        if (triggerZone != null) triggerZone.radius = detectRange;
    }

    /// <summary>Adds a trigger SphereCollider sized to detectRange if one isn't already present.</summary>
    private void EnsureTriggerZone()
    {
        foreach (var s in GetComponents<SphereCollider>())
        {
            if (s.isTrigger) { triggerZone = s; break; }
        }
        if (triggerZone == null)
        {
            triggerZone = gameObject.AddComponent<SphereCollider>();
            triggerZone.isTrigger = true;
        }
        triggerZone.radius = detectRange;
    }

    // ---------- Trigger callbacks ---------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (owner == null || owner.Consumed) return;
        var receiver = other.GetComponentInParent<IItemReceiver>();
        if (receiver == null) return;
        inside = receiver;
        TryLock();
    }

    /// <summary>
    /// Re-checks each fixed update while a receiver is inside but not yet qualified.
    /// Cheap — only fires for objects currently in the trigger, not scene-wide.
    /// Handles the case where the receiver becomes eligible mid-overlap (e.g. takes damage
    /// while standing on a HP pickup that earlier rejected because HP was full).
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        if (isPulling) return;
        if (owner == null || owner.Consumed) return;
        if (inside == null) return;
        TryLock();
    }

    private void OnTriggerExit(Collider other)
    {
        var receiver = other.GetComponentInParent<IItemReceiver>();
        if (receiver != null && ReferenceEquals(receiver, inside))
        {
            inside = null;
            if (!isPulling) lockedReceiver = null;
        }
    }

    private void TryLock()
    {
        if (inside == null) return;
        if (!owner.CanBeReceivedBy(inside)) return;
        lockedReceiver = inside;
        isPulling = true;
    }

    // ---------- Pull loop ------------------------------------------------

    private void Update()
    {
        if (!isPulling) return;
        if (owner == null || owner.Consumed) return;

        if (armTimer < armDelay) { armTimer += Time.deltaTime; return; }
        PullTowardReceiver();
    }

    private void PullTowardReceiver()
    {
        if (lockedReceiver == null) { isPulling = false; return; }

        rb.useGravity = false;
        if (mainCollider != null) mainCollider.enabled = false;

        Vector3 toReceiver = lockedReceiver.transform.position - transform.position;
        rb.AddForce(toReceiver.normalized * pullStrength, ForceMode.VelocityChange);
        if (rb.linearVelocity.magnitude > maxPullSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxPullSpeed;

        if (toReceiver.magnitude < snapDistance)
        {
            if (owner.TryConsume(lockedReceiver))
                OnConsumed();
        }
    }

    // ---------- Editor visualization -------------------------------------

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
