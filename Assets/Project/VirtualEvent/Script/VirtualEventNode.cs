using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public abstract class VirtualEventNode : MonoBehaviour
{
    protected OnDrawGizmosTriggerEvent onDrawGizmosTriggerEvent = new OnDrawGizmosTriggerEvent();

    [SerializeField] protected bool isEnableGizmos;

    public virtual void Execute()
    {
        //Execute all next node
        if (this.nextVirtualEventNode != null && this.nextVirtualEventNode.Length > 0)
        {
            for (int i = 0; i < this.nextVirtualEventNode.Length; i++)
            {
                this.nextVirtualEventNode[i].Execute();
            }
        }

        if(this.triggerUnityEvent != null)
            this.triggerUnityEvent.Invoke();
    }

    protected virtual void OnValidate()
    {
        this.onDrawGizmosTriggerEvent.isDrawEnable = this.isEnableGizmos;
    }

    public float drawNameDistance = 7; 
    protected virtual void OnDrawGizmos()
    {
        if(isEnableGizmos == false)
            return;

        Vector3 cameraPos;
        if (Application.isPlaying){
            cameraPos = Camera.main.transform.position;
        }
        else{
            cameraPos = SceneView.lastActiveSceneView.camera.transform.position;
        }
        if (Vector3.Distance(cameraPos, this.transform.position) < this.drawNameDistance){
            Handles.Label(this.transform.position + (Vector3.up * .3f), this.name);
        }

        Gizmos.color = color;
        Gizmos.DrawSphere(this.transform.position,  .25f);

        if (this.nextVirtualEventNode != null && this.nextVirtualEventNode.Length > 0)
        {
            for (int i = 0; i < this.nextVirtualEventNode.Length; i++)
            {
                if (this.nextVirtualEventNode[i] != null)
                this.onDrawGizmosTriggerEvent.DrawSphere(this.transform.position, this.nextVirtualEventNode[i].transform.position, this.color);
            }
        }

        if (this.triggerUnityEvent != null)
            this.onDrawGizmosTriggerEvent.DrawGizmosEvent(this.transform, this.triggerUnityEvent, this.color);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(this.transform.position, (Vector3.up * .3f));

    }

    [SerializeField] public VirtualEventNode[] nextVirtualEventNode;
    public UnityEvent triggerUnityEvent;
    [SerializeField] protected Color color;
}
