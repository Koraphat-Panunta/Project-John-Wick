using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    public static readonly float reach;

    public Vector3 capsuleColliderCenterOffset;
    public Vector3 capsuleColliderCenterPosition => this.transform.position + this.capsuleColliderCenterOffset;
    public float raduis;
    public float height;

    public LayerMask layerMask;

    public Vector3 hitPos;

    public Vector3 topPoint;
    public Vector3 bottomPoint;
   
    public void Move(Vector3 motion)
    {
        const int maxIterations = 5;
        const float skinWidth = 0.02f;

        Vector3 remainingMotion = motion;

        for (int i = 0; i < maxIterations; i++)
        {
            if (remainingMotion.sqrMagnitude < 0.000001f)
                break;

            Vector3 capsuleCenter = capsuleColliderCenterPosition;
            float halfHeight = Mathf.Max(0, height / 2f - raduis);

            Vector3 capsuleTop = capsuleCenter + Vector3.up * halfHeight;
            Vector3 capsuleBottom = capsuleCenter - Vector3.up * halfHeight;

            Vector3 direction = remainingMotion.normalized;
            float distance = remainingMotion.magnitude + skinWidth;

            bool hit = Physics.CapsuleCast(
                capsuleTop,
                capsuleBottom,
                raduis,
                direction,
                out RaycastHit hitInfo,
                distance,
                layerMask,
                QueryTriggerInteraction.Ignore
            );

            if (!hit)
            {
                // Free movement
                transform.position += remainingMotion;
                break;
            }

            // --- MOVE UP TO HIT POINT ---
            float moveDistance = Mathf.Max(hitInfo.distance - skinWidth, 0f);
            Vector3 moveToHit = direction * moveDistance;
            transform.position += moveToHit;

            // Debug
            //Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.red, 2f);

            // --- SLIDE ALONG SURFACE ---
            remainingMotion -= moveToHit;

            remainingMotion = Vector3.ProjectOnPlane(
                remainingMotion,
                hitInfo.normal
            );

            // Prevent jitter on near-parallel surfaces
            if (remainingMotion.sqrMagnitude < 0.000001f)
                break;
        }

    }
    private void OnDrawGizmos()
    {
        DrawCapsuleGizmo(capsuleColliderCenterPosition, this.height,this.raduis,Color.blue);
    }

    public void DrawCapsuleGizmo(
    Vector3 center,
    float height,
    float radius,
    Color color)
    {
        Gizmos.color = color;

        float halfHeight = Mathf.Max(0, height / 2f - radius);

        Vector3 top = center + Vector3.up * halfHeight;
        Vector3 bottom = center - Vector3.up * halfHeight;

        // Spheres
        Gizmos.DrawWireSphere(top, radius);
        Gizmos.DrawWireSphere(bottom, radius);

        // Lines
        Gizmos.DrawLine(top + Vector3.forward * radius, bottom + Vector3.forward * radius);
        Gizmos.DrawLine(top - Vector3.forward * radius, bottom - Vector3.forward * radius);
        Gizmos.DrawLine(top + Vector3.right * radius, bottom + Vector3.right * radius);
        Gizmos.DrawLine(top - Vector3.right * radius, bottom - Vector3.right * radius);

        Gizmos.color = Color.red * .5f;
        Gizmos.DrawSphere(center, .15f);

        Gizmos.color = Color.aliceBlue * .5f;
        Gizmos.DrawSphere(hitPos, .15f);
        Gizmos.DrawSphere(topPoint, .15f);
        Gizmos.DrawSphere(bottomPoint, .15f);
    }
}
