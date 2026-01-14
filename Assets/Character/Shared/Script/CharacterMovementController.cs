using System;
using Unity.Cinemachine;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    public enum GroundState
    {
        OnLinear,
        OnSlope,
        Stall
    }

    [Header("Ground & Gravity")]
    public bool isEnableGravity = true;
    private float gravityScale = 0.01f;
    public float gravity => 9.81f * this.gravityScale;
    public float maxSlopeAngle = 45f;

    //[Header("Step")]
    //public float stepHeight = 0.35f;

    [Header("Debug")]
    public GroundState groundState;
    public Vector3 groundNormal;
    public bool isGrounded;

    [SerializeField] private Vector3 verticalDownGravityVelocity;
    private float maxVerticalDownGravityVelocity = 5;

    public static readonly float reach;

    public Vector3 capsuleColliderCenterOffset;
    public Vector3 capsuleColliderCenterPosition => this.transform.position + this.capsuleColliderCenterOffset;
    public float raduis;
    public float height;
    float halfHeight => Mathf.Max(0, height / 2f - raduis);

    public LayerMask layerMask;

    public Vector3 topPoint => capsuleColliderCenterPosition + Vector3.up * halfHeight;
    public Vector3 bottomPoint => capsuleColliderCenterPosition - Vector3.up * halfHeight;


   
    private void MoveUpdate(Vector3 motion)
    {
        
        Vector3 remainingMotion = motion;

        const int maxIterations = 5;
        const float skinWidth = 0.02f;

        for (int i = 0; i < maxIterations; i++)
        {

            if (remainingMotion.sqrMagnitude < 0.000001f)
                break;


            float halfHeight = Mathf.Max(0, height / 2f - raduis);



            Vector3 direction = remainingMotion.normalized;
            float distance = remainingMotion.magnitude + skinWidth;

            //Debug.DrawRay(capsuleBottom, direction * distance,Color.red);

            bool hit = Physics.CapsuleCast(
                this.topPoint,
                this.bottomPoint,
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
                this.transform.position += remainingMotion;
                break;
            }

            // --- MOVE UP TO HIT POINT ---
            float moveDistance = Mathf.Max(hitInfo.distance - skinWidth, 0f);
            Vector3 moveToHit = direction * moveDistance;
            this.transform.position += moveToHit;

            // Debug

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

    public void Move(Vector3 motion)
    {
        float castDistance = (height / 2) + raduis + (Mathf.Sin(maxSlopeAngle * Mathf.Deg2Rad) * raduis) + .02f;
        Vector3 castPos = startCast + (motion.normalized * raduis);
        Vector3 remainingMotion = Vector3.ProjectOnPlane(motion, groundNormal);

        //Debug.DrawRay(castPos, Vector3.down * castDistance, Color.green);

        if (Physics.Raycast(castPos,Vector3.down,out RaycastHit hitInfo, castDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            float slopeAngle = Vector3.Angle(hitInfo.normal, Vector3.up);

            Vector3 projectMotionOnNormal = Vector3.ProjectOnPlane(motion, hitInfo.normal);
            //Debug.DrawRay(castPos, projectMotionOnNormal, Color.green);

            if (Vector3.Dot(Vector3.up,projectMotionOnNormal.normalized) < 0
                && slopeAngle >= 5 
                && slopeAngle <= maxSlopeAngle)
                remainingMotion = projectMotionOnNormal;
            
        } 

        this.MoveUpdate(remainingMotion);
    }

    public void SetCharacterControllerAttribute(CharacterMovementControllerScriptableObject characterMovementControllerScriptableObject)
    {
        this.capsuleColliderCenterOffset = characterMovementControllerScriptableObject.centerOffsetPosition;
        this.maxSlopeAngle = characterMovementControllerScriptableObject.slopeAngle;
        this.height = characterMovementControllerScriptableObject.height;
        this.raduis = characterMovementControllerScriptableObject.raduis;

    }

   
    private void FixedUpdate()
    {
        this.UpdateGroundState();
        this.UpdateGravity();
    }

    Vector3 startCast => capsuleColliderCenterPosition + (Vector3.up * raduis);
    float castDistance => (height/2) + 0.05f;

    
    private void UpdateGroundState()
    {

        if(Physics.SphereCast(this.startCast,raduis,Vector3.down,out RaycastHit hit, this.castDistance, this.layerMask, QueryTriggerInteraction.Ignore))
        {
            groundNormal = hit.normal;
            //Debug.DrawLine(startCast, hit.point, Color.yellow);
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle < 5)
            {
                Debug.Log("OnLinear");
                groundState = GroundState.OnLinear;
                this.isGrounded = true;
                if(this.transform.position.y < hit.point.y)
                {
                    this.transform.position = new Vector3(this.transform.position.x, hit.point.y + .02f, this.transform.position.z);
                }

            }
            else if (slopeAngle <= maxSlopeAngle)
            {
                Debug.Log("OnSlope");
                groundState = GroundState.OnSlope;
                this.isGrounded = true;
                if (this.transform.position.y < hit.point.y - .02f)
                {
                    this.transform.position = new Vector3(this.transform.position.x, hit.point.y - .02f, this.transform.position.z);
                }
            }
            else
            {
                Debug.Log("Stall OnSlope Angle = " + slopeAngle);
                groundState = GroundState.Stall;
                this.isGrounded = false;
            }
        }
        else
        {
            Debug.Log("Stall");
            this.groundState = GroundState.Stall;
            this.isGrounded = false;
            groundNormal = Vector3.zero;
        }
    }
    private void UpdateGravity()
    {
        if (this.isGrounded == true || this.isEnableGravity == false)
        {
            this.verticalDownGravityVelocity = Vector3.zero;
            return;
        }


        float velocityY = Mathf.Clamp(this.verticalDownGravityVelocity.y - (this.gravity * Time.fixedDeltaTime)
               , -maxVerticalDownGravityVelocity
               , maxVerticalDownGravityVelocity);

        this.verticalDownGravityVelocity = new Vector3(
            0
            , velocityY
            , 0);


        this.MoveUpdate(this.verticalDownGravityVelocity);
    }

    [SerializeField] protected bool isEnableGizmos;

    private void OnDrawGizmos()
    {
        if(this.isEnableGizmos == false)
            return;
            
        DrawCapsuleGizmo(capsuleColliderCenterPosition, this.height, this.raduis, Color.green);
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

        //Gizmos.color = Color.aliceBlue * .5f;
        //Gizmos.DrawSphere(topPoint, .15f);
        //Gizmos.DrawSphere(bottomPoint, .15f);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(startCast, Vector3.down * castDistance);
    }
}
