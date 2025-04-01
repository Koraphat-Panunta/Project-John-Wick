using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;


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

    [Range(0,10)]
    [SerializeField] private float collisionRaduisCheck;

    [SerializeField] private Transform targetFollow;
    [SerializeField] private Transform targetLook;
    [Range(0,360)]
    [SerializeField] private float yaw;
    [Range(-90,90)]
    [SerializeField] private float pitch;
    [Range(30,90)]
    [SerializeField] private float maxPitch;
    [Range(-30,-90)]
    [SerializeField] private float minPitch;

    private void Awake()
    {
        transform.SetParent(null,true);
    }
    void Start()
    {
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void Update()
    {
        UpdateCameraPosition();
    }
   
    void InputRotateCamera(float horizontalInput,float verticalInput)
    {
        yaw += horizontalInput * rotationSpeed;
        pitch -= verticalInput * rotationSpeed;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    void UpdateCameraPosition()
    {
        Vector3 desiredPosition = transform.position;
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = targetFollow.position + rotation * (cameradistance * distance);
        transform.LookAt(targetLook.position);
        transform.position += transform.right * cameraOffset.x + transform.up * cameraOffset.y + transform.forward * cameraOffset.z;
        desiredPosition = transform.position;

        Vector3 startCastPos = targetLook.position + transform.right * cameraOffset.x + transform.up * cameraOffset.y + transform.forward * cameraOffset.z;
        Vector3 castDir = transform.position - startCastPos;
        Debug.DrawLine(startCastPos, startCastPos + transform.position);

        if (Physics.Raycast(startCastPos, castDir.normalized, out RaycastHit hit,castDir.magnitude, collisionLayers))
        {
            desiredPosition = hit.point + (transform.forward * collisionPushForward); /*+ transform.right * cameraOffset.x + transform.up * cameraOffset.y + transform.forward * cameraOffset.z;*/
            transform.position = desiredPosition;
        }
        
    }
    private void OnValidate()
    {

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = targetFollow.position + rotation * (cameradistance*distance);
        transform.LookAt(targetLook.position);

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



