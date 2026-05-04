using UnityEngine;

/// <summary>
/// Auto-magnet pickup. OverlapSphere scans for IItemReceiver each frame; once one is
/// in range AND wants the item, the pickup is pulled toward it via physics force, then
/// consumed when within snap distance. Math ported from the legacy ItemObject.Update().
/// </summary>
[RequireComponent(typeof(Pickupable))]
[RequireComponent(typeof(Rigidbody))]
public class MagnetPickupBehavior : MonoBehaviour, IPickupBehavior
{
    [Range(0, 10)] [SerializeField] private float detectRange = 3f;
    [Range(0, 100)] [SerializeField] private float pullStrength = 8f;
    [Range(0, 100)] [SerializeField] private float maxPullSpeed = 12f;
    [SerializeField] private float snapDistance = 0.45f;
    [Tooltip("Brief warm-up before this pickup is allowed to be magneted (lets the spawn impulse settle).")]
    [SerializeField] private float armDelay = 0.25f;
    [SerializeField] private Collider mainCollider;

    private Pickupable owner;
    private Rigidbody rb;
    private IItemReceiver lockedReceiver;
    private bool isPulling;
    private float armTimer;

    public void Initialize(Pickupable o) { owner = o; }
    public void OnConsumed() { Destroy(gameObject); }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (mainCollider == null) mainCollider = GetComponent<Collider>();
    }

    private void OnEnable() { armTimer = 0f; isPulling = false; lockedReceiver = null; }

    private void Update()
    {
        if (owner == null || owner.Consumed) return;

        if (armTimer < armDelay) armTimer += Time.deltaTime;

        if (!isPulling) TryDetectReceiver();
        if (isPulling && armTimer >= armDelay) PullTowardReceiver();
    }

    private void TryDetectReceiver()
    {
        var hits = Physics.OverlapSphere(transform.position, detectRange);
        for (int i = 0; i < hits.Length; i++)
        {
            var receiver = hits[i].GetComponentInParent<IItemReceiver>();
            if (receiver == null) continue;
            if (!owner.CanBeReceivedBy(receiver)) continue;

            lockedReceiver = receiver;
            isPulling = true;
            return;
        }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
