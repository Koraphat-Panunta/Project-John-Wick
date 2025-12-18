using UnityEngine;
using UnityEngine.Events;

public class LessVirtualEvent : VirtualEventNode
{
    [SerializeField] protected NumberVirtualVariable numberVirtualVariable;
    [SerializeField] protected NumberVirtualVariable overrideCompareValue;
    [SerializeField] protected float compareValue;
    [SerializeField] protected bool equalCompare;
    public override void Execute()
    {
        if (this.equalCompare)
        {
            if (this.numberVirtualVariable._value <= (this.overrideCompareValue ? this.overrideCompareValue._value : this.compareValue))
                base.Execute();
            else
            {
                this.OnFalseExecute();
            }
        }
        else
        {
            if (this.numberVirtualVariable._value < (this.overrideCompareValue ? this.overrideCompareValue._value : this.compareValue))
                base.Execute();
            else
            {
                this.OnFalseExecute();
            }
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
        if (this.isEnableGizmos == false)
            return;

        if (this.numberVirtualVariable != null)
        {
            this.onDrawGizmosTriggerEvent.DrawLine(this.numberVirtualVariable.transform.position, this.transform.position, this.numberVirtualVariable.color);
            base.DrawName(Vector3.Lerp(this.transform.position, this.numberVirtualVariable.transform.position, .35f), "GetValue");
        }

        if (this.overrideCompareValue != null)
        {
            this.onDrawGizmosTriggerEvent.DrawLine(this.overrideCompareValue.transform.position, this.transform.position, this.overrideCompareValue.color);
            base.DrawName(Vector3.Lerp(this.transform.position, this.overrideCompareValue.transform.position, .35f), "CompareValue");
        }


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
