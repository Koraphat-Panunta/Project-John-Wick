using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

using System;




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
    public float collisionPushForward = 0.5f;
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
        transform.SetParent(null,true);
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
    public void UpdateCameraPosition(Vector3 targetFollow,Vector3 targetLookAt)
    {
        if(isBeenUpdate == true)
            return;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = targetFollow + rotation * (cameradistance * distance);
        transform.LookAt(targetLookAt);
        transform.position += transform.right * cameraOffset.x + transform.up * cameraOffset.y + transform.forward * cameraOffset.z;

        Vector3 startCastPos = targetLookAt + transform.right * cameraOffset.x + transform.up * cameraOffset.y;
        Vector3 castDir = transform.position - startCastPos;

        //CheckCameraBeenBlocked
        if(Physics.Raycast(targetLookAt, (transform.position - targetLookAt).normalized, out RaycastHit hit, (transform.position - targetLookAt).magnitude+0.2f, collisionLayers))
        {
            Vector3 camToHitDir = hit.point - transform.position;

            Debug.DrawLine(hit.point,transform.position,Color.red);
            float angle = Vector3.Angle(transform.forward, camToHitDir.normalized);

            float moveForward = camToHitDir.magnitude * Mathf.Cos(Mathf.Deg2Rad*angle) + collisionPushForward;
            Debug.DrawLine(transform.position, transform.position + transform.forward * moveForward, Color.blue);

            transform.position += transform.forward * moveForward + (hit.normal*collisionPushForward);

            if(Physics.Raycast(targetLookAt, (transform.position-targetLookAt).normalized, out RaycastHit hitSecond, (transform.position - targetLookAt).magnitude + 0.2f, collisionLayers))
            {
                transform.position = hitSecond.point + (hitSecond.normal* collisionPushForward);
            }
        }
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
        //Gizmos.DrawWireSphere(transform.position, collisionRaduisCheck);

        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(transform.position + transform.forward * -collisionRaduisCheck, (transform.position + transform.forward * -collisionRaduisCheck)+transform.forward*collisionPushForward);

    }
#endif
}



