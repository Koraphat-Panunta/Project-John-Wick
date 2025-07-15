using UnityEngine;

public abstract class InWorldUI : MonoBehaviour
{
    [SerializeField] protected Canvas _canvas;
    public abstract Vector3 _anchorPos {  get; }
    public abstract Vector3 _offsetUIPos { get; }
    public Camera _lookToCamera { get; private set; }

    protected virtual void Awake()
    {
        this._canvas = GetComponent<Canvas>();
        this._lookToCamera = Camera.main;
    }
    protected virtual void FixedUpdate()
    {    
        this.UpdatePosition();
    }
   
    public void SetCameraLookAt(Camera camera)
    {
        this._lookToCamera = camera;
    }
    private void UpdatePosition()
    {
        transform.position = _anchorPos + _offsetUIPos;
        transform.rotation = Quaternion.LookRotation
            (_lookToCamera.transform.position - transform.position, Vector3.up);
    }
    public void EnableUI()
    {
        this.enabled = true;
    }
    public void DisableUI()
    {
        this.enabled = false;
    }
}
