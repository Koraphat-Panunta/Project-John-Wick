using System;
using System.Collections;
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

    private struct VirtualTriangle
    {
        public Vector3 vertexForward;
        public Vector3 vertexBackLeft;
        public Vector3 vertexBackRight;
        public Vector3 normal;
        public float slopeAngle;
        public Vector3 centroid;
    }

    [Header("Ground & Gravity")]
    public bool enableGravity = true;
    private float gravityScale = 1;
    public float gravity => 9.81f * this.gravityScale;

    public static readonly float SKIN_WIDTH_THREASHORED = .02f;

    //[Header("Step")]
    //public float stepHeight = 0.35f;

    [Header("Debug")]
    public GroundState groundState;
    public Vector3 groundNormal;
    public bool isGrounded;
    private VirtualTriangle _groundTriangle;

    [SerializeField] public Vector3 velocityPhysicBased;
    private float maxVerticalDownGravityVelocity = 10;

    public static readonly float reach;

    public float maxSlopeAngle = 45f;
    

    [SerializeField] protected CharacterMovementControllerScriptableObject characterMovementControllerScriptableObject;
    [SerializeField] CapsuleCollider capsuleCollider;
    public bool enableDynamicCollider;
    public Vector3 capsuleColliderCenterOffset { get => this.enableDynamicCollider
            ?this.capsuleCollider.center
            :this.characterMovementControllerScriptableObject.centerOffsetPosition; 
    }
    public float raduis { get => this.enableDynamicCollider
            ?this.capsuleCollider.radius
            :this.characterMovementControllerScriptableObject.raduis; 
    }
    public float height { get => this.enableDynamicCollider
            ?this.capsuleCollider.height
            : this.characterMovementControllerScriptableObject.height; 
    }

    public Vector3 capsuleColliderCenterPosition => this.position + this.capsuleColliderCenterOffset;

    float halfHeight => Mathf.Max(0, height / 2f - raduis);

    public LayerMask layerMask;
    public LayerMask characterCollideLayerMask;

    public Vector3 topPoint => capsuleColliderCenterPosition + Vector3.up * halfHeight;
    public Vector3 bottomPoint => capsuleColliderCenterPosition - Vector3.up * halfHeight;

    public Vector3 position;
    public Quaternion rotation;
   
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
                this.layerMask,
                QueryTriggerInteraction.Ignore
            );

            if (!hit)
            {
                // Free movement
                this.position += remainingMotion;
                break;
            }

            // --- MOVE UP TO HIT POINT ---
            float moveDistance = Mathf.Max(hitInfo.distance - skinWidth, 0f);
            Vector3 moveToHit = direction * moveDistance;
            this.position += moveToHit;

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

    private readonly Collider[] _overlapBuffer = new Collider[16];

    private float psuhBackCharacterForce = 1.5f;
    public bool enableCharacterCollide = true;
    private void CharacterCollideCheck()//Check CharacterCollideEachOther
    {
        if(this.enableCharacterCollide == false)
            return;

        int count = Physics.OverlapCapsuleNonAlloc(this.topPoint, this.bottomPoint, this.raduis, _overlapBuffer, this.characterCollideLayerMask, QueryTriggerInteraction.Ignore);
        Vector3 moveMotion = Vector3.zero;

        for (int i = 0; i < count; i++)
        {
            if (_overlapBuffer[i].TryGetComponent<CharacterMovementController>(out CharacterMovementController characterMovementController)
                && characterMovementController == this)
                continue;

            Vector3 dirPush = this.transform.transform.position - _overlapBuffer[i].transform.position;
            moveMotion += new Vector3(dirPush.x, 0, dirPush.z).normalized;


        }

        moveMotion = moveMotion.normalized * this.psuhBackCharacterForce * Time.fixedDeltaTime;
        MoveUpdate(moveMotion);
    }
    private float psuhBackObsCharacterForce = 5;
    private void ObstacleCollideCheck()//Check CharacterCollideEachOther
    {
        if (this.enableCharacterCollide == false)
            return;

        int count = Physics.OverlapCapsuleNonAlloc(this.topPoint, this.bottomPoint, this.raduis, _overlapBuffer, this.layerMask, QueryTriggerInteraction.Ignore);
        Vector3 moveMotion = Vector3.zero;

        if (count == 0)
            return;

        for (int i = 0; i < count; i++)
        {
            Vector3 castDir = (_overlapBuffer[i].transform.position - this.startCast);
            Vector3 dirPush = Vector3.zero;

            if (Physics.Raycast(this.startCast
                , castDir.normalized
                , out RaycastHit hitInfo
                , castDir.magnitude
                , layerMask
                , QueryTriggerInteraction.Ignore))
            {
               dirPush = hitInfo.normal;
            }
            dirPush = this.transform.transform.position - _overlapBuffer[i].transform.position;
            moveMotion += new Vector3(dirPush.x, 0, dirPush.z).normalized;


        }

        moveMotion = moveMotion.normalized * this.psuhBackObsCharacterForce * Time.fixedDeltaTime;
        MoveUpdate(moveMotion);
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
    public void SetRotation(Quaternion quaternion)
    {
        this.rotation = Quaternion.Euler(0, quaternion.eulerAngles.y, 0);
    }
    public void SetCharacterControllerAttribute(CharacterMovementControllerScriptableObject characterMovementControllerScriptableObject)
    {
        this.characterMovementControllerScriptableObject = characterMovementControllerScriptableObject;
        this.maxSlopeAngle = characterMovementControllerScriptableObject.slopeAngle;

    }
    public CapsuleCollider GetCharacterCapsuleCollider()
    {
        return this.capsuleCollider;
    }
    private void Awake()
    {
        this.position = transform.position;
        this.transformPositionCheck = transform.localPosition;
    }
    private void Start()
    {
        this.lastPos = this.position;
    }

    private Vector3 lastPos;
    public Vector3 curVelocity;
    private void Update()
    {
        this.UpdateGravity();
        this.UpdateGroundState();


        Vector3 currentPos = this.position;

        Vector3 deltaPos = currentPos - lastPos;

        this.curVelocity = deltaPos / Time.deltaTime;

        this.lastPos = currentPos;

        this.UpdateCharacterRotation();

    }

    

    private Vector3 transformPositionCheck;
    private Quaternion transformRotationCheck;
    private void FixedUpdate()
    {
        this.CharacterCollideCheck();
        this.ObstacleCollideCheck();
        this.MoveUpdate(this.velocityPhysicBased * Time.fixedDeltaTime);
        this.UpdateCharacterPosition();
    }

    public void UpdateCharacterPosition()
    {
        if (this.transform.position != this.transformPositionCheck)
        {
            Debug.LogWarning("Transform position been update corrpt" + "obj "+this.gameObject);
            this.position = this.transform.position;
        }

        this.transform.position = this.position;
        this.transformPositionCheck = this.transform.position;
    }

    public void UpdateCharacterRotation()
    {
        if (this.transformRotationCheck != this.transform.rotation)
        {
            Debug.LogWarning("Transform rotation been update corrpt" + "obj " + this.gameObject);
            this.rotation = this.transform.rotation;
        }

        this.transform.rotation = this.rotation;
        this.transformRotationCheck = this.transform.rotation;
    }

    Vector3 startCast => capsuleColliderCenterPosition + (Vector3.up * raduis);
    float castDistance => (height/2) + 0.05f;

    [SerializeField] protected bool isUpdateGround = true;
    public void SetGroundUpdate(bool value) => this.isUpdateGround = value;
    private void UpdateGroundState()
    {
        if (this.isUpdateGround == false)
            return;

        UpdateGroundTriangle();

        Vector3 placePosition = ProjectPointOntoPlane(this.position, _groundTriangle.centroid, _groundTriangle.normal);





        Debug.DrawLine(_groundTriangle.vertexForward, _groundTriangle.vertexBackLeft, Color.cyan);
        Debug.DrawLine(_groundTriangle.vertexBackLeft, _groundTriangle.vertexBackRight, Color.cyan);
        Debug.DrawLine(_groundTriangle.vertexBackRight, _groundTriangle.vertexForward, Color.cyan);
        Debug.DrawRay(_groundTriangle.centroid, _groundTriangle.normal * 0.5f, Color.magenta);
        Debug.DrawLine(this.position, placePosition, Color.yellow);

        switch (this.groundState)
        {
            case GroundState.Stall:
                {
                    if (!Physics.SphereCast(
                        this.startCast, raduis, Vector3.down,
                        out RaycastHit hit,
                        this.castDistance - .002f ,
                        this.layerMask, QueryTriggerInteraction.Ignore)
                        ||this.stallExitAble == false)
                    {
                        break;
                    }

                    this.groundNormal = Vector3.zero;
                    this.UpdateGroundStatePositionOnGround(hit.point);

                }
                break;
            case GroundState.OnLinear: 
                {
                    if (!Physics.SphereCast(
                       this.startCast, raduis, Vector3.down,
                       out RaycastHit hit,
                       this.castDistance + this.characterMovementControllerScriptableObject.maxStepHeight + 0.5f,
                       this.layerMask, QueryTriggerInteraction.Ignore))
                    {
                        this.TriggerStall();
                        break;
                    }

                    this.groundNormal = _groundTriangle.normal;
                    this.UpdateGroundStatePositionOnGround( hit.point);
                }
                break;
            case GroundState.OnSlope:
                {
                    if (!Physics.SphereCast(
                        this.startCast, raduis, Vector3.down,           
                        out RaycastHit hit,            
                        this.castDistance + this.characterMovementControllerScriptableObject.maxStepHeight + 0.5f,
                        this.layerMask, QueryTriggerInteraction.Ignore))
                    {
                        this.TriggerStall();
                        break;
                    }

                    this.groundNormal = _groundTriangle.normal;
                    this.UpdateGroundStatePositionOnGround(placePosition);
                }
                break;
            
        }

   
    }

    private void UpdateGroundStatePositionOnGround(Vector3 placePosition)
    {
       
        if (_groundTriangle.slopeAngle < 5)
        {
            this.groundState = GroundState.OnLinear;
            this.isGrounded = true;

            //if (this.position.y < placePosition.y + SKIN_WIDTH_THREASHORED)
            this.position = Vector3.Lerp(this.position, new Vector3(this.position.x, placePosition.y + SKIN_WIDTH_THREASHORED, this.position.z),Time.deltaTime * 10);

        }
        else if (_groundTriangle.slopeAngle <= maxSlopeAngle)
        {
            this.groundState = GroundState.OnSlope;
            this.isGrounded = true;
            if (this.position.y < placePosition.y - SKIN_WIDTH_THREASHORED)
                this.position = new Vector3(this.position.x, placePosition.y - SKIN_WIDTH_THREASHORED , this.position.z);


        }
        else
        {
            this.groundState = GroundState.Stall;
            this.isGrounded = false;
            this.groundNormal = Vector3.zero;

        }
    }

    private void UpdateGroundTriangle()
    {
        Vector3 forwardDir = this.rotation * Vector3.forward;
        forwardDir.y = 0;
        forwardDir.Normalize();

        float vertexDistance = this.raduis * 2;
        Vector3 castStartForward   = this.startCast + (forwardDir * vertexDistance);
        Vector3 castStartBackLeft  = this.startCast + (Quaternion.Euler(0, -135, 0) * forwardDir * vertexDistance);
        Vector3 castStartBackRight = this.startCast + (Quaternion.Euler(0,  135, 0) * forwardDir * vertexDistance);
        
        Vector3 terrainForward = GetGroundVertex(castStartForward);
        Vector3 terrainBackLeft = GetGroundVertex(castStartBackLeft);
        Vector3 terrainBackRight = GetGroundVertex(castStartBackRight);
        Vector3 terrainCenter = GetGroundVertex(this.startCast);

        Vector3 middleBack = (terrainBackLeft + terrainBackRight) / 2f;

        Vector3 middlePlane = (terrainForward + terrainBackLeft + terrainBackRight)/3f;

        if (middlePlane.y < terrainCenter.y)
        {
            if(Mathf.Abs(terrainCenter.y - terrainForward.y) <= .005f
                || Mathf.Abs(terrainCenter.y - middleBack.y) <= .005f)
            {
                terrainForward = new Vector3(terrainForward.x, terrainCenter.y, terrainForward.z);
                terrainBackLeft = new Vector3(terrainBackLeft.x, terrainCenter.y, terrainBackLeft.z);
                terrainBackRight = new Vector3(terrainBackRight.x, terrainCenter.y, terrainBackRight.z);
            }
            
        }

        _groundTriangle.vertexBackLeft  = terrainBackLeft;
        _groundTriangle.vertexBackRight = terrainBackRight;
        _groundTriangle.vertexForward = terrainForward;

        Vector3 edge1 = _groundTriangle.vertexForward   - _groundTriangle.vertexBackLeft;
        Vector3 edge2 = _groundTriangle.vertexBackRight - _groundTriangle.vertexBackLeft;
        Vector3 normal = Vector3.Cross(edge2, edge1).normalized;

        if (normal.y < 0)
            normal = -normal;

        _groundTriangle.normal = normal;
        _groundTriangle.slopeAngle = Vector3.Angle(normal, Vector3.up);
        _groundTriangle.centroid = (_groundTriangle.vertexForward + _groundTriangle.vertexBackLeft + _groundTriangle.vertexBackRight) / 3f;


    }

    /// <summary>
    /// Project จุดลงบน plane โดยใช้ normal ของ plane
    /// คืนค่าตำแหน่งที่ X, Z คงเดิม แต่ Y อยู่บน plane
    /// </summary>
    /// <param name="point">จุดที่ต้องการ project</param>
    /// <param name="pointOnPlane">จุดใดๆ ที่อยู่บน plane (เช่น triangle centroid)</param>
    /// <param name="planeNormal">normal ของ plane</param>
    /// <returns>ตำแหน่งที่ projected บน plane</returns>
    private Vector3 ProjectPointOntoPlane(Vector3 point, Vector3 pointOnPlane, Vector3 planeNormal)
    {
        // ป้องกันกรณี normal เป็นแนวนอนสมบูรณ์ (จะหารด้วยศูนย์)
        if (Mathf.Abs(planeNormal.y) < 0.0001f)
            return point;

        // สมการ plane: N·(P - P0) = 0
        // เมื่อเรารู้ X และ Z แล้วต้องหา Y:
        // N.x*(P.x - P0.x) + N.y*(P.y - P0.y) + N.z*(P.z - P0.z) = 0
        // N.y*(P.y - P0.y) = -(N.x*(P.x - P0.x) + N.z*(P.z - P0.z))
        // P.y = P0.y - (N.x*(P.x - P0.x) + N.z*(P.z - P0.z)) / N.y

        float projectedY = pointOnPlane.y -
            (planeNormal.x * (point.x - pointOnPlane.x) +
             planeNormal.z * (point.z - pointOnPlane.z)) / planeNormal.y;

        return new Vector3(point.x, projectedY, point.z);
    }

    /// <summary>
    /// ยิง ray ลงเพื่อหาจุดบนพื้น (vertex) สำหรับการคำนวณ virtual ramp
    /// </summary>
    private Vector3 GetGroundVertex(Vector3 origin)
    {
        float maxCastDistance = this.castDistance + this.characterMovementControllerScriptableObject.maxStepHeight + 0.5f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, maxCastDistance, this.layerMask, QueryTriggerInteraction.Ignore))
        {
            return hit.point;
        }

        return origin + Vector3.down * maxCastDistance;
    }
    public void SetVelocityPhysicBased(Vector3 v) => this.velocityPhysicBased = v;
   
    private void UpdateGravity()
    {
        if (this.isGrounded == true 
            || this.enableGravity == false
            || this.groundState == GroundState.OnLinear
            || this.groundState == GroundState.OnSlope)
        {
                this.velocityPhysicBased = new Vector3(this.velocityPhysicBased.x,0,this.velocityPhysicBased.z);
            return;
        }


        float velocityY = Mathf.Clamp(this.velocityPhysicBased.y - (this.gravity * Time.deltaTime)
               , -maxVerticalDownGravityVelocity
               , maxVerticalDownGravityVelocity);

        this.velocityPhysicBased = new Vector3(
            0
            , velocityY
            , 0);
    }
    public void PushForceUp(float force,float velocityChangeDuration)
    {
        Debug.Log("PushForceUp");
        this.TriggerStall();
        this.velocityPhysicBased = new Vector3(this.velocityPhysicBased.x, force, this.velocityPhysicBased.z);
    }
    public void TriggerStall()
    {
        this.groundState = GroundState.Stall;
        this.isGrounded = false;
        this.groundNormal = Vector3.zero;
        this.stallExitAble = false;
        this.StartCoroutine(StallBufferTime());
    }
    private bool stallExitAble;
    private IEnumerator StallBufferTime()
    {
        yield return new WaitForSeconds(.5f);
        this.stallExitAble = true;
    }
    private IEnumerator VelocityChange(float force, float velocityChangeDuration)
    {
        float time = 0;

        Vector3 enterV = this.velocityPhysicBased;

        while (time < velocityChangeDuration)
        {
            if(velocityChangeDuration == 0)
                break;

            this.velocityPhysicBased = Vector3.Lerp(enterV, new Vector3(this.velocityPhysicBased.x, force, this.velocityPhysicBased.z),time/velocityChangeDuration) ;
            yield return null;
        }

        this.velocityPhysicBased = new Vector3(this.velocityPhysicBased.x, force, this.velocityPhysicBased.z);
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
