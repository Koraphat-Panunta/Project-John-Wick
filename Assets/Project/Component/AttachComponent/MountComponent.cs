using UnityEngine;

public class MountComponent : MonoBehaviour
{
    [SerializeField] private Transform attachAbleObject;
    public Transform _attachAbleObject { get => this.attachAbleObject; protected set => attachAbleObject = value; }
    public Transform _parentAttachTransform { get => this.parentAttachTransform; protected set => this.parentAttachTransform = value; }
    public Transform parentAttachTransform;
    public Vector3 offsetPosition;
    public Quaternion offsetRotation = Quaternion.identity;

    private bool isEnableAutoAttachRate = true;
    public bool _isEnableAutoAttachRate { get => this.isEnableAutoAttachRate; protected set => this.isEnableAutoAttachRate = value; }

    [Range(0, 1)]
    [SerializeField] protected float attachRate;

    [Range(0,10)]
    [SerializeField] protected float attachDuration;
    public float _attachDuration { get => attachDuration; protected set => attachDuration = value; }

    private void Update()
    {
       this.UpdatePosition();
    }
    protected virtual void LateUpdate()
    {
        //this.parentAttachTransform = _parentAttachTransform;
        if (_parentAttachTransform != null)
        {
            if (_isEnableAutoAttachRate)
            {
                if (_attachDuration <= 0)
                    attachRate = 1;
                else
                    attachRate = Mathf.Clamp01(attachRate + (Time.deltaTime * (1 / _attachDuration)));
            }

           this.UpdatePosition();
        }
    }
    public void UpdatePosition()
    {
        Quaternion targetRot = GetAttachRotation();

        // Check if the target rotation is nearly zero (invalid)
        if (Mathf.Approximately(targetRot.x, 0) && Mathf.Approximately(targetRot.y, 0) &&
            Mathf.Approximately(targetRot.z, 0) && Mathf.Approximately(targetRot.w, 0))
        {
            targetRot = Quaternion.identity;
        }

        _attachAbleObject.position = Vector3.Lerp(_attachAbleObject.position, GetAttachPosition(), this.attachRate);
        _attachAbleObject.rotation = Quaternion.Lerp(_attachAbleObject.rotation, targetRot, this.attachRate);
    }
    public virtual void Attach(Transform parentTransform, float attatchingDuration)
    {
        this.Attach(parentTransform, this.offsetPosition, this.offsetRotation);
        this.SetAttachDuration(attatchingDuration);
    }
    public virtual void Attach(Transform parentTransform, Vector3 offsetPosition,Quaternion offsetRotation,float attatchingDuration)
    {
        this.Attach(parentTransform, offsetPosition, offsetRotation);
        this.SetAttachDuration(attatchingDuration); 
    }
    public void SetOffsetPosition(Vector3 offsetPosition) => this.offsetPosition = offsetPosition;
    public void SetOffserRotation(Quaternion offsetRotation) => this.offsetRotation = offsetRotation;
    public virtual void Attach(Transform parentTransform, Vector3 offsetPosition, Quaternion offsetRotation)
    {
        
        this.offsetPosition = offsetPosition;
        this.offsetRotation = offsetRotation;

        this._parentAttachTransform = parentTransform;

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
    public void SetAttachAbleObject(Transform attachAbleObject) => this.attachAbleObject = attachAbleObject;
    public virtual void Detach()
    {
        this._parentAttachTransform = null;
        attachRate = 0;
    }

    public virtual Vector3 GetAttachPosition()
    {
        if(_parentAttachTransform == null)
            return Vector3.zero;

        Vector3 attachPosition = _parentAttachTransform.position;

        return attachPosition 
            + (_parentAttachTransform.forward * offsetPosition.z)
            + (_parentAttachTransform.right * offsetPosition.x)
            + (_parentAttachTransform.up * offsetPosition.y);
        
    }
    public virtual Quaternion GetAttachRotation()
    {

        if (_parentAttachTransform == null)
            return Quaternion.identity;

        try
        {
            return _parentAttachTransform.rotation * offsetRotation;
        }
        catch
        {
            return Quaternion.identity;
        }
       
    }

    public void SetAttachRate(float r) => this.attachRate = Mathf.Clamp01(r);

  
}
