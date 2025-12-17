using UnityEngine;

public class SetBoolVirtualVariable : VirtualEventNode
{
    [SerializeField] protected BoolVirtualVariable setBoolVirtualVariable;
    [SerializeField] protected BoolVirtualVariable overrideBoolValue;
    [SerializeField] private bool setBool;

    public override void Execute()
    {
        if(this.overrideBoolValue != null)
        {
            this.setBoolVirtualVariable.value = this.overrideBoolValue.value;
        }
        else
        {
            this.setBoolVirtualVariable.value = this.setBool;
        }
        base.Execute();
    }

    protected override void OnDrawGizmos()
    {
        if(isEnableGizmos == false)
            return;

        if(this.setBoolVirtualVariable != null)
        {
            this.onDrawGizmosTriggerEvent.DrawLine(this.setBoolVirtualVariable.transform.position, this.transform.position, color);
            base.DrawName(Vector3.Lerp(this.transform.position, this.setBoolVirtualVariable.transform.position, .35f),"SetBool");
        }

        if(this.overrideBoolValue != null)
        {
            this.onDrawGizmosTriggerEvent.DrawLine(this.overrideBoolValue.transform.position, this.transform.position, color);
            base.DrawName(Vector3.Lerp(this.transform.position, this.overrideBoolValue.transform.position, .35f), "OverrideBool");
        }

        base.OnDrawGizmos();
    }
}
