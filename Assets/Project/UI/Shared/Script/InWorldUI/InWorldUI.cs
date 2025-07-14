using UnityEngine;

public class InWorldUI : MonoBehaviour
{
    public Canvas canvas;
    public Vector3 anchorPos;
    public Vector3 offsetUIPos;
    public Camera lookToCamera { get; private set; }

    private void Awake()
    {
        this.canvas = GetComponent<Canvas>();
        this.lookToCamera = Camera.main;
    }
    protected virtual void FixedUpdate()
    {
        this.UpdatePosition();
    }
    public void SetAnchorPosition(Vector3 anchorPos)
    {
        this.anchorPos = anchorPos;
    }
    public void SetOffsetPosition(Vector3 offsetPosition)
    {
        this.offsetUIPos = offsetPosition;
    }
    public void SetCameraLookAt(Camera camera)
    {
        this.lookToCamera = camera;
    }
    private void UpdatePosition()
    {
        transform.position = anchorPos + offsetUIPos;
        transform.rotation = Quaternion.LookRotation
            (lookToCamera.transform.position - transform.position, Vector3.up);
    }
}
