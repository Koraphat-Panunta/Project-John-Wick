using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class SnapperLevel : MonoBehaviour
{
    [Header("Snap Settings")]
    [SerializeField] protected Transform snapTransform;
    [SerializeField] protected float snapRadius = 0.25f;

    [Header("Debug")]
    [SerializeField] protected SnapperLevel snapAnchor;

    Vector3 lastPosition;
    Quaternion lastRotation;
    [SerializeField] bool isSnapped;

    // --------------------------------------
    void OnEnable()
    {
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }
    private bool isMovingLast;
    void Update()
    {
#if UNITY_EDITOR
        if (Application.isPlaying) return;

        bool isMoving =
            transform.position != lastPosition ||
            transform.rotation != lastRotation;

        if (this.snapAnchor != null)
        {
            if(Vector3.Distance(this.transform.position,this.snapAnchor.transform.position) > snapRadius)
                Unsnap();
        }
        else
            FindingSnapAnchor();



        if (this.isMovingLast 
            && isMoving == false
            && snapAnchor != null)
        {
            SnapTo(snapAnchor);
        }

        isMovingLast = isMoving;

        lastPosition = transform.position;
        lastRotation = transform.rotation;
#endif
    }

    // --------------------------------------
    void FindingSnapAnchor()
    {
        if (isSnapped) return;
        if (snapTransform == null) return;

        int layerMask = LayerMask.GetMask("SnapSocket");

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            snapRadius,
            layerMask,
            QueryTriggerInteraction.Collide
        );

        float closest = snapRadius;
        SnapperLevel best = null;

        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent(out SnapperLevel other)) continue;
            if (other == this) continue;
            if (other.snapTransform == null) continue;

            float dist = Vector3.Distance(
                transform.position,
                other.transform.position
            );

            if (dist < closest)
            {
                closest = dist;
                best = other;
            }
        }

        if (best != null)
        {
            this.snapAnchor = best;
            best.snapAnchor = this;
        }
        else
            this.snapAnchor = null;
    }

    // --------------------------------------
    void SnapTo(SnapperLevel target)
    {
        snapAnchor = target;
        isSnapped = true;


        // POSITION
        snapTransform.rotation = Quaternion.LookRotation(target.transform.forward * (Vector3.Dot(target.transform.forward, snapTransform.forward) > 0 ? 1f : -1f), target.transform.up); 

        Vector3 deltaPos = snapTransform.position - transform.position;
        snapTransform.position = target.transform.position + deltaPos;



        target.OnSnap(this);
    }

    // --------------------------------------
    void Unsnap()
    {
        if (snapAnchor != null)
        {
            snapAnchor.snapAnchor = null;
            snapAnchor.isSnapped = false;
        }

        snapAnchor = null;
        isSnapped = false;
    }

    // --------------------------------------
    public void OnSnap(SnapperLevel other)
    {
        snapAnchor = other;
        isSnapped = true;
    }

    // --------------------------------------
    void OnDrawGizmos()
    {
        if(isSnapped)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, .15f);


        if (snapAnchor != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position,snapAnchor.transform.position);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * this.snapRadius);
        }
    }
}