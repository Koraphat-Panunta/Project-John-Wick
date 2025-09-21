using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

using System;
using UnityEngine.Animations.Rigging;





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

    public CinemachineCamera cinemachineCamera;

    [Range(0,10)]
    [SerializeField] private float collisionRaduisCheck;

    [SerializeField] private Transform targetFollow;
    public Transform targetFollowTarget { get => targetFollow; protected set => this.targetFollowTarget = value; }
    [SerializeField] private Transform targetLook;
    public Transform targetLookTarget { get => targetLook; protected set => this.targetLookTarget = value; }

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
    }
    private void LateUpdate()
    {
        isBeenUpdate = false;
    }

   
   
    public void InputRotateCamera(float horizontalInput,float verticalInput)
    {
        yaw += horizontalInput * rotationSpeed;
        pitch -= verticalInput * rotationSpeed;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }
    public void SetYaw(float value)=>this.yaw = value;
    public void SetPitch(float value)=> this.pitch = Mathf.Clamp(value,minPitch,maxPitch);

    public void UpdateCameraPosition() => UpdateCameraPosition(this.targetFollow.position, this.targetLook.position);
    private float trackingRate;
    public void UpdateCameraPosition(Vector3 targetFollow,Vector3 targetLookAt)
    {
        if(isBeenUpdate == true)
            return;

        Vector3 targetPos;
        Vector3 targetDir;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        targetPos = targetFollow + rotation * (cameradistance * distance);
        targetDir = (targetLookAt - targetPos).normalized;

        Vector3 camForward = targetDir;
        Vector3 camRight = Vector3.Cross(Vector3.up, camForward).normalized;
        Vector3 camUp = Vector3.Cross(camForward, camRight).normalized;

        // Apply offset in camera's local space
        targetPos += camRight * cameraOffset.x;
        targetPos += camUp * cameraOffset.y;
        targetPos += camForward * cameraOffset.z;



        //Vector3 startCastPos = targetLookAt + transform.right * cameraOffset.x + transform.up * cameraOffset.y;
        //Vector3 castDir = transform.position - startCastPos;

       

        Vector3 nearCenter = targetPos + (targetDir * cinemachineCamera.Lens.NearClipPlane);

        ////CheckCameraBeenBlocked
        Vector3 targetFollowTargetPosition = this.targetFollowTarget.position;
        if (Physics.Raycast(targetFollowTargetPosition ,(targetPos - targetFollowTargetPosition).normalized, out RaycastHit hit, (targetPos - targetFollowTargetPosition).magnitude, collisionLayers, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(targetFollowTargetPosition,hit.point,Color.red);

            Vector3 camPosToHitpoint = hit.point - targetPos;

            float angle = Vector3.Angle(camPosToHitpoint, hit.normal);

            

            Debug.DrawRay(targetPos, targetDir * camPosToHitpoint.magnitude, Color.green);

            targetPos += targetDir * (camPosToHitpoint.magnitude * Mathf.Cos(angle * Mathf.Deg2Rad));

            Debug.DrawLine(hit.point,targetPos,Color.blue);
        }


        ////Check CameraBeenCollide

        //nearCenter = targetPos + (targetDir * cinemachineCamera.Lens.NearClipPlane);

        //Vector3 nearLeft = nearCenter - (camRight * halfWidth);
        //Vector3 nearRight = nearCenter + (camRight * halfWidth);

        Vector3 sideTargerAdd = Vector3.zero;

        //if (Physics.Raycast(targetPos, camRight * -1, out RaycastHit hitLeft, .2f, collisionLayers, QueryTriggerInteraction.Ignore))
        //{
        //    sideTargerAdd += camRight * 0.2f;
        //}

        //if (Physics.Raycast(targetPos, camRight, out RaycastHit hitRight, .2f, collisionLayers, QueryTriggerInteraction.Ignore))
        //{
        //    sideTargerAdd += camRight * -0.2f;
        //}

        //targetPos += sideTargerAdd;

        transform.position = targetPos;
        transform.rotation = Quaternion.LookRotation(targetDir);
        isBeenUpdate = true;
    }
    
    //private CancellationTokenSource cancellationTokenSource;
    //public async void UpdateTargetFollowLookAt(Vector3 targetFollow, Vector3 targetLookAt,float transitionDuration)
    //{
    //    if(cancellationTokenSource != null)
    //    {
    //        cancellationTokenSource.Cancel();
    //        cancellationTokenSource.Dispose();
    //    }

    //    cancellationTokenSource = new CancellationTokenSource();
    //    CancellationToken token = cancellationTokenSource.Token;

    //    float t = 0;
    //    Vector3 beginTargetFollow = curFollowTarget;
    //    Vector3 beginTargetLookAt = curLookTarget;
    //    try
    //    {
    //        while (t <= transitionDuration)
    //        {
    //            token.ThrowIfCancellationRequested();

    //            t += Time.deltaTime;
    //            this.curFollowTarget = Vector3.Lerp(beginTargetFollow, targetFollow, t);
    //            this.curLookTarget = Vector3.Lerp(beginTargetLookAt, targetLookAt, t);
    //            await Task.Yield();
    //        }
    //    }
    //    catch (OperationCanceledException) 
    //    {
    //        Debug.Log("UpdateTargetFollowLookAt was cancelled.");
    //    }
    //}


    private void OnValidate()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = targetFollow.position + rotation * (cameradistance*distance);
        transform.LookAt(targetLook);

        transform.position += transform.right * cameraOffset.x + transform.up * cameraOffset.y + transform.forward * cameraOffset.z;
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.02f, 0.455f, 0.851f);
        Gizmos.DrawWireSphere(targetFollow.position, distance);

        //Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(_transform.position, collisionRaduisCheck);

        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(_transform.position + _transform.forward * -collisionRaduisCheck, (_transform.position + _transform.forward * -collisionRaduisCheck)+_transform.forward*collisionPushForward);

    }
#endif
}



