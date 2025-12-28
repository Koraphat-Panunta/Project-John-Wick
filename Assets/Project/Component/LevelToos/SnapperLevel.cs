using UnityEngine;
using UnityEngine.InputSystem;


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
    [SerializeField] bool isSnappedValue;
    protected bool getIsSnapped 
    {
        get 
        {
            if(snapAnchor == null)
                isSnappedValue = false;
            return isSnappedValue;
        }
        set
        {
            this.isSnappedValue = value;
        }
    }

    // --------------------------------------
    void OnEnable()
    {
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }
    private bool isMovingLast;
    public bool isMoving { get; protected set; }
    void Update()
    {
#if UNITY_EDITOR
        if (Application.isPlaying) return;

        this.isMoving =
            transform.position != lastPosition ||
            transform.rotation != lastRotation;

        if (this.getIsSnapped)
        {
            if(this.isMoving)
            this.Unsnap();

            if(snapAnchor != null 
                && snapAnchor.snapAnchor != this)
                snapAnchor = null;
        }
        else
        {
            FindingSnapAnchor();
            if (this.isMovingLast
           && this.isMoving == false
           && snapAnchor != null
           )
            {
                SnapTo(snapAnchor);
            }
        }

        isMovingLast = this.isMoving;

        lastPosition = transform.position;
        lastRotation = transform.rotation;
#endif
    }

   
    // --------------------------------------
    void FindingSnapAnchor()
    {
        if (getIsSnapped) 
            return;
        if (snapTransform == null) 
            return;


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
            if (hit.TryGetComponent(out SnapperLevel other) == false) 
                continue;
            if (other == this) 
                continue;
            if (other.snapTransform == null) 
                continue;
            if (other.snapTransform == this.snapTransform)
                continue;
            if(other.getIsSnapped)
                continue;
            if(other.snapAnchor != null)
                continue;
            if(other.isMoving)
                continue;

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
        getIsSnapped = true;

        Quaternion addRotForward = Quaternion.FromToRotation(transform.forward, target.transform.forward * -1);
        snapTransform.rotation *= addRotForward;

        Quaternion addRotUpward = Quaternion.FromToRotation(transform.up, target.transform.up );
        snapTransform.rotation *= addRotUpward;

        Vector3 deltaPos = snapTransform.position - transform.position;
        snapTransform.position = target.transform.position + deltaPos;


        target.OnSnap(this);
    }

    // --------------------------------------
    void Unsnap()
    {
        if(snapAnchor != null)
        snapAnchor.OnUnSnap(this);

        snapAnchor = null;
        getIsSnapped = false;
    }
    public void OnUnSnap(SnapperLevel snapper)
    {
        snapAnchor = null;
        getIsSnapped = false;
    }
    // --------------------------------------
    public void OnSnap(SnapperLevel other)
    {
        snapAnchor = other;
        getIsSnapped = true;
    }

    // --------------------------------------
    void OnDrawGizmos()
    {
        if(getIsSnapped)
            return;

        Gizmos.color = new Color(0.639f, 0.875f,1);
        Gizmos.DrawWireSphere(transform.position, .15f);


        if (snapAnchor != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position,snapAnchor.transform.position);
        }
        else
        {
            if (isMoving)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, transform.forward * this.snapRadius);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.forward * .2f);

            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.up * .2f);
        }
       
    }

   


}