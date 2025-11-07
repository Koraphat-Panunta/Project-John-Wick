using UnityEngine;

public class MountComponent : MonoBehaviour
{
    [SerializeField] private Transform attachAbleObject;
    public Transform _attachAbleObject { get => this.attachAbleObject; protected set => attachAbleObject = value; }
    public Transform parentAttachTransform { get; protected set; }
    public Vector3 offsetPosition;
    public Quaternion offsetRotation;

    private bool isEnableAutoAttachRate = true;
    public bool _isEnableAutoAttachRate { get => this.isEnableAutoAttachRate; protected set => this.isEnableAutoAttachRate = value; }

    [Range(0, 1)]
    private float attachRate;

    [Range(0,10)]
    [SerializeField] private float attachDuration;
    public float _attachDuration { get => attachDuration; protected set => attachDuration = value; }

    protected virtual void Update()
    {
        if(parentAttachTransform != null)
        {
            if(_isEnableAutoAttachRate)
                attachRate = Mathf.Clamp01(attachRate + (Time.deltaTime * (1/_attachDuration)));

            _attachAbleObject.position = Vector3.Lerp(_attachAbleObject.position,GetAttachPosition(),this.attachRate);
            _attachAbleObject.rotation = Quaternion.Lerp(_attachAbleObject.rotation,GetAttachRotation(), this.attachRate);
        }
    }

    public virtual void Attach(Transform parentTransform, Vector3 offsetPosition,Quaternion offsetRotation)
    {
        this.parentAttachTransform = parentTransform;
        this.offsetPosition = offsetPosition;
        this.offsetRotation = offsetRotation;
        attachRate = 0;
    }
    public void EnableAutoAttachRate() => _isEnableAutoAttachRate = true;
    public void DisableAutoAttachRate() => _isEnableAutoAttachRate = false;
   
    public void Hold(Vector3 holdPosition, Quaternion holdRotation,float holdRate)
    {
        _attachAbleObject.position = Vector3.Lerp(_attachAbleObject.position, holdPosition, holdRate);
        _attachAbleObject.rotation = Quaternion.Lerp(_attachAbleObject.rotation, holdRotation, holdRate);
    }
    public void SetAttachDuration(float duration) => this._attachDuration = duration;
    public virtual void Detach()
    {
        this.parentAttachTransform = null;
        attachRate = 0;
    }

    public virtual Vector3 GetAttachPosition()
    {
        if(parentAttachTransform == null)
            return Vector3.zero;

        Vector3 attachPosition = parentAttachTransform.position;

        return attachPosition 
            + (parentAttachTransform.forward * offsetPosition.z)
            + (parentAttachTransform.right * offsetPosition.x)
            + (parentAttachTransform.up * offsetPosition.y);
        
    }
    public virtual Quaternion GetAttachRotation()
    {
        if (parentAttachTransform == null)
            return Quaternion.identity;

        return parentAttachTransform.rotation * offsetRotation;
    }
}
