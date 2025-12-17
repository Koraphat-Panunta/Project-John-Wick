using UnityEngine;
using UnityEngine.Events;

public class BranchVirtualEvent : VirtualEventNode
{
    [SerializeField] protected BoolVirtualVariable boolVirtualVariable;

    public override void Execute()
    {
        if(boolVirtualVariable.value == true)
        base.Execute();
        else
        {
            this.OnFalseExecute();
        }
    }

    [SerializeField] public VirtualEventNode[] onFalseVirtualEventNode;
    public UnityEvent onFalsetriggerUnityEvent;
    public void OnFalseExecute()
    {
        //Execute all next node
        if (this.onFalseVirtualEventNode != null && this.onFalseVirtualEventNode.Length > 0)
        {
            for (int i = 0; i < this.onFalseVirtualEventNode.Length; i++)
            {
                this.onFalseVirtualEventNode[i].Execute();
            }
        }

        if (this.onFalsetriggerUnityEvent != null)
            this.onFalsetriggerUnityEvent.Invoke();
    }

    [SerializeField] private Color falseColor;
    protected override void OnDrawGizmos()
    {
        if(this.isEnableGizmos == false)
            return;

        if(this.boolVirtualVariable != null)
            this.onDrawGizmosTriggerEvent.DrawLine
                (this.boolVirtualVariable.transform.position
                , this.transform.position
                , this.boolVirtualVariable.color
                );


        if (this.onFalseVirtualEventNode != null && this.onFalseVirtualEventNode.Length > 0)
        {
            for (int i = 0; i < this.onFalseVirtualEventNode.Length; i++)
            {
                if (this.onFalseVirtualEventNode[i] != null)
                    this.onDrawGizmosTriggerEvent.DrawSphere(this.transform.position, this.onFalseVirtualEventNode[i].transform.position, this.falseColor);
            }
        }

        if (this.onFalsetriggerUnityEvent != null)
            this.onDrawGizmosTriggerEvent.DrawGizmosEvent(this.transform, this.onFalsetriggerUnityEvent, this.falseColor);




        base.OnDrawGizmos();
    }
}
