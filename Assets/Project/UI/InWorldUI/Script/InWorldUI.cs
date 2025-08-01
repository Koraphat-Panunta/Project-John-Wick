using UnityEngine;

public class InWorldUI : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] protected Canvas _canvas;
    public Vector3 _anchorPos;
    public Vector3 _offsetUIPos;
    public Camera _lookToCamera { get; private set; }

    protected virtual void Awake()
    {
        this.animator = GetComponent<Animator>();
        this._canvas = GetComponent<Canvas>();
        this._lookToCamera = Camera.main;
    }
    protected virtual void FixedUpdate()
    {    
        this.UpdateTransform();
    }
    public void SetAnchorPosition(Vector3 anchorPos)
    {
        this._anchorPos = anchorPos;
    }
    public void SetOffsetPosition(Vector3 offset)
    {
        this._offsetUIPos = offset;
    }
    public void PlayAnimation(string animationName)
    {
        this.animator.CrossFade(animationName,0.05f,0);
    }
    public void SetCameraLookAt(Camera camera)
    {
        this._lookToCamera = camera;
    }
    private void UpdateTransform()
    {
        transform.position = _anchorPos + _offsetUIPos;
        transform.rotation = Quaternion.LookRotation
            ((_lookToCamera.transform.position - transform.position).normalized, Vector3.up);
    }
    private void OnValidate()
    {
        if(this._canvas == null)
        {
            if(gameObject.TryGetComponent<Canvas>(out Canvas canvasComponent))
                this._canvas = canvasComponent;
        }
    }

}
