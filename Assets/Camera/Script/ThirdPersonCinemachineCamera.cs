using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

using System;
using UnityEngine.Animations.Rigging;
using System.Data;


#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(CinemachineCamera))]
public class ThirdPersonCinemachineCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public Vector3 cameraOffset;
    [Range(0,10)]
    public float distance;
    private Vector3 cameradistance = new Vector3(0,0,1);
    public float rotationSpeed = 3f;
    public float collisionPushForward = 0;
    public LayerMask collisionLayers;

    [Header("Debug")]
    public bool showCollisionDebug = true;

    public CinemachineCamera cinemachineCamera;

    [Range(0,10)]
    [SerializeField] private float collisionRaduisCheck;

    [SerializeField] private Transform targetFollow;
    public Transform targetFollowTarget { get => targetFollow; protected set => this.targetFollowTarget = value; }
    [SerializeField] private Transform targetLook;
    public Transform targetLookTarget { get => targetLook; protected set => this.targetLookTarget = value; }

    private Vector3 _trackLocalOffset;
    private Vector3 _lookLocalOffset;

    // World-space position computed from local offset — moves with targetFollowTarget automatically.
    public Vector3 curTrackPosition => targetFollowTarget != null
        ? targetFollowTarget.TransformPoint(_trackLocalOffset) : _trackLocalOffset;
    // World-space position computed from local offset — moves with targetLookTarget automatically.
    public Vector3 curLookPosition  => targetLookTarget  != null
        ? targetLookTarget.TransformPoint(_lookLocalOffset)  : _lookLocalOffset;

    [Range(0,360)]
    [SerializeField] public float yaw;
    [Range(-90,90)]
    [SerializeField] public float pitch;
    [Range(30,90)]
    [SerializeField] private float maxPitch;
    [Range(-30,-90)]
    [SerializeField] private float minPitch;

    private bool isBeenUpdate;

    private void Awake()
    {
        //transform.SetParent(null,true);
    }

    void Start()
    {
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
        _trackLocalOffset = Vector3.zero;
        _lookLocalOffset  = Vector3.zero;
    }

    private void LateUpdate()
    {
        this.transform.position = this.targetPos;
        this.transform.rotation = Quaternion.LookRotation(this.targetDir);
        isBeenUpdate = false;
    }

    public void InputRotateCamera(float horizontalInput, float verticalInput)
    {
        this.SetYaw(yaw += (horizontalInput * rotationSpeed));
        this.SetPitch(pitch -= (verticalInput * rotationSpeed));
    }

    public void InputRotateCameraToDirection(Vector3 lookDirection)
    {
        // Compute the direction from the follow target (camera pivot) to the lookAt position
        if (lookDirection == Vector3.zero)
            return;

        lookDirection.Normalize();

        Quaternion rotation = Quaternion.LookRotation(lookDirection * -1, Vector3.up);

        float rawYaw = rotation.eulerAngles.y;

        float rawPitch = rotation.eulerAngles.x;
        if (rawPitch > 180f)
            rawPitch -= 360f;

        yaw = rawYaw;
        pitch = Mathf.Clamp(rawPitch, minPitch, maxPitch);
    }

    public void InputRotateCamera(Vector3 lookAtPosition)
    {
        // Compute the direction from the follow target (camera pivot) to the lookAt position
        Vector3 lookDir = (lookAtPosition - targetFollow.position);
        if (lookDir == Vector3.zero)
            return;

        this.InputRotateCameraToDirection(lookDir);
    }

    public void RotateCameraTowardsDirection(Vector3 lookDirection, float rotateSpeed)
    {
        if (lookDirection == Vector3.zero)
            return;

        lookDirection.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection * -1, Vector3.up);

        float targetYaw = targetRotation.eulerAngles.y;
        float targetPitch = targetRotation.eulerAngles.x;
        if (targetPitch > 180f)
            targetPitch -= 360f;

        targetPitch = Mathf.Clamp(targetPitch, minPitch, maxPitch);

        float step = rotateSpeed * Time.deltaTime;

        yaw = Mathf.MoveTowardsAngle(yaw, targetYaw, step);
        pitch = Mathf.MoveTowards(pitch, targetPitch, step);
    }

    public void SetYaw(float value) => this.yaw = value;
    public void SetPitch(float value) => this.pitch = Mathf.Clamp(value, minPitch, maxPitch);

    // Uses current stored local offsets — does not snap to raw target transforms.
    public void UpdateCameraPosition()
    {
        if (isBeenUpdate) return;
        UpdateCameraPositionInternal(curTrackPosition, curLookPosition);
    }

    private float trackingRate;

    // Converts incoming world positions to local offsets, then computes the camera.
    public void UpdateCameraPosition(Vector3 trackWorld, Vector3 lookWorld)
    {
        if (isBeenUpdate) return;
        _trackLocalOffset = targetFollowTarget.InverseTransformPoint(trackWorld);
        _lookLocalOffset  = targetLookTarget.InverseTransformPoint(lookWorld);
        UpdateCameraPositionInternal(trackWorld, lookWorld);
    }

    private void UpdateCameraPositionInternal(Vector3 trackWorld, Vector3 lookWorld)
    {
        Vector3 targetPos;
        Vector3 targetDir;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        targetPos = trackWorld + rotation * (cameradistance * distance);
        targetDir = (lookWorld - targetPos).normalized;

        Vector3 camForward = targetDir;
        Vector3 camRight = Vector3.Cross(Vector3.up, camForward).normalized;
        Vector3 camUp = Vector3.Cross(camForward, camRight).normalized;

        // Apply offset in camera's local space
        targetPos += camRight * cameraOffset.x;
        targetPos += camUp * cameraOffset.y;
        targetPos += camForward * cameraOffset.z;

        Vector3 nearCenter = targetPos + (targetDir * cinemachineCamera.Lens.NearClipPlane);

        float collideSphereRaduis = 0.15f;

        // Collision always casts from the real follow target, not the animated pivot
        Vector3 targetFollowTargetPosition = this.targetFollowTarget.position + (Vector3.up * 0.5f) + (camUp * cameraOffset.y);

#if UNITY_EDITOR
        _dbgCastFrom = targetFollowTargetPosition;
        _dbgDesiredPos = targetPos;
        _dbgSphereHit = false;
        _dbgRayHit = false;
#endif

        if (Physics.SphereCast(
            targetFollowTargetPosition,
            collideSphereRaduis,
            (targetPos - targetFollowTargetPosition).normalized,
            out RaycastHit hitInfo,
            (targetPos - targetFollowTargetPosition).magnitude,
            LayerMask.GetMask("Default"),
            QueryTriggerInteraction.Ignore))
        {
#if UNITY_EDITOR
            _dbgSphereHit = true;
            _dbgHitPoint = hitInfo.point;
            _dbgHitNormal = hitInfo.normal;
            if (showCollisionDebug)
            {
                Debug.DrawLine(targetFollowTargetPosition, hitInfo.point, Color.red);
                Debug.DrawRay(hitInfo.point, hitInfo.normal * 0.3f, Color.yellow);
            }
#endif
            targetPos = hitInfo.point + (hitInfo.normal * collideSphereRaduis);
        }
        else
        {
            if (Physics.Raycast(
                targetFollowTargetPosition,
                (targetPos - targetFollowTargetPosition).normalized,
                out RaycastHit hitInfoRay,
                (targetPos - targetFollowTargetPosition).magnitude + collideSphereRaduis,
                LayerMask.GetMask("Default"),
                QueryTriggerInteraction.Ignore))
            {
#if UNITY_EDITOR
                _dbgRayHit = true;
                _dbgHitPoint = hitInfoRay.point;
                _dbgHitNormal = hitInfoRay.normal;
                if (showCollisionDebug)
                {
                    Debug.DrawLine(targetFollowTargetPosition, hitInfoRay.point, Color.magenta);
                    Debug.DrawRay(hitInfoRay.point, hitInfoRay.normal * 0.3f, Color.yellow);
                }
#endif
                targetPos = hitInfoRay.point + (hitInfoRay.normal * collideSphereRaduis);
            }
#if UNITY_EDITOR
            else if (showCollisionDebug)
            {
                Debug.DrawLine(targetFollowTargetPosition, targetPos, Color.green);
            }
#endif
        }

        this.targetPos = targetPos;
        this.targetDir = targetDir;
        isBeenUpdate = true;
    }

    public Vector3 targetPos { get; protected set; }
    public Vector3 targetDir { get; protected set; }

#if UNITY_EDITOR
    private Vector3 _dbgCastFrom;
    private Vector3 _dbgDesiredPos;
    private bool _dbgSphereHit;
    private bool _dbgRayHit;
    private Vector3 _dbgHitPoint;
    private Vector3 _dbgHitNormal;
    private const float _dbgSphereRadius = 0.15f;
#endif

    private void OnValidate()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = targetFollow.position + rotation * (cameradistance * distance);
        transform.LookAt(targetLook);

        transform.position += transform.right * cameraOffset.x + transform.up * cameraOffset.y + transform.forward * cameraOffset.z;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!showCollisionDebug) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.08f);

        if (targetLook != null)
        {
            Gizmos.color = new Color(0f, 0.8f, 1f, 0.5f);
            Gizmos.DrawLine(transform.position, targetLook.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!showCollisionDebug || targetFollow == null) return;

        Gizmos.color = new Color(1f, 1f, 0f, 0.8f);
        Gizmos.DrawWireSphere(_dbgCastFrom, _dbgSphereRadius);

        if (Application.isPlaying)
        {
            Vector3 castDir = (_dbgDesiredPos - _dbgCastFrom).normalized;
            float castDist = (_dbgDesiredPos - _dbgCastFrom).magnitude;

            if (_dbgSphereHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_dbgCastFrom, _dbgHitPoint);
                Gizmos.DrawWireSphere(_dbgHitPoint, _dbgSphereRadius);

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_dbgHitPoint, _dbgHitPoint + _dbgHitNormal * 0.4f);

                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(targetPos, _dbgSphereRadius);
                Gizmos.DrawLine(_dbgHitPoint, targetPos);
            }
            else if (_dbgRayHit)
            {
                Gizmos.color = new Color(1f, 0.2f, 0.8f);
                Gizmos.DrawLine(_dbgCastFrom, _dbgHitPoint);
                Gizmos.DrawWireSphere(_dbgHitPoint, 0.05f);

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_dbgHitPoint, _dbgHitPoint + _dbgHitNormal * 0.4f);

                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(targetPos, _dbgSphereRadius);
                Gizmos.DrawLine(_dbgHitPoint, targetPos);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_dbgCastFrom, _dbgDesiredPos);
                Gizmos.DrawWireSphere(_dbgDesiredPos, _dbgSphereRadius);
            }
        }
        else
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 desiredPos = targetFollow.position + rotation * (cameradistance * distance);
            desiredPos += transform.right * cameraOffset.x + transform.up * cameraOffset.y + transform.forward * cameraOffset.z;

            Gizmos.color = new Color(0f, 1f, 0.4f, 0.6f);
            Gizmos.DrawLine(targetFollow.position, desiredPos);
            Gizmos.DrawWireSphere(targetFollow.position, _dbgSphereRadius);
            Gizmos.DrawWireSphere(desiredPos, _dbgSphereRadius);
        }

        UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.3f,
            _dbgSphereHit ? "[Collision: SphereCast]" :
            _dbgRayHit   ? "[Collision: Raycast]"    :
            Application.isPlaying ? "[No Collision]" : "[Edit Mode]");
    }
#endif
}
