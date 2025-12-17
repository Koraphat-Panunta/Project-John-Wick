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
    protected void DrawName(Vector3 position,string name)
    {
        Vector3 cameraPos;
        if (Application.isPlaying)
        {
            cameraPos = Camera.main.transform.position;
        }
        else
        {
            cameraPos = SceneView.lastActiveSceneView.camera.transform.position;
        }
        if (Vector3.Distance(cameraPos, position) < this.drawNameDistance)
        {
            Handles.Label(position + (Vector3.up * .3f), name);
            Gizmos.color = Color.black;
            Gizmos.DrawRay(position, (Vector3.up * .3f));
        }
    }
    protected virtual void OnDrawGizmos()
    {
        if(isEnableGizmos == false)
            return;

        DrawName(this.transform.position,this.name);

        Gizmos.color = color;
        Gizmos.DrawSphere(this.transform.position,  .2f);

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



    }

    [SerializeField] public VirtualEventNode[] nextVirtualEventNode;
    public UnityEvent triggerUnityEvent;
    [SerializeField] protected Color color;
}
