using UnityEngine;

public class OperateNumberVirtualEvent : VirtualEventNode
{
    [SerializeField] protected NumberVirtualVariable setNumberVirtualVariable;
    [SerializeField] protected NumberVirtualVariable overrideOperateNumberValue;
    [SerializeField] private float operateNumberValue;


    public enum OperateNumber
    {
        Set,
        Add,
        Subtract,
        Multiply,
        Divide,
    }

    [SerializeField] protected OperateNumber operateNumber;

    public override void Execute()
    {
        float operateValue = this.overrideOperateNumberValue ? this.overrideOperateNumberValue._value : this.operateNumberValue;

        switch (this.operateNumber)
        {
            case OperateNumber.Set: setNumberVirtualVariable._value = operateValue; 
                break;
            case OperateNumber.Add: setNumberVirtualVariable._value += operateValue;
                break;
            case OperateNumber.Subtract: setNumberVirtualVariable._value -= operateValue;
                break;
            case OperateNumber.Multiply: setNumberVirtualVariable._value *= operateValue;
                break;
            case OperateNumber.Divide: setNumberVirtualVariable._value /= operateValue;
                break;
        }
       
        base.Execute();
    }

    protected override void OnDrawGizmos()
    {
        if (isEnableGizmos == false)
            return;

        if (this.setNumberVirtualVariable != null)
        {
            this.onDrawGizmosTriggerEvent.DrawLine(this.setNumberVirtualVariable.transform.position, this.transform.position, this.setNumberVirtualVariable.color);
            base.DrawName(Vector3.Lerp(this.transform.position, this.setNumberVirtualVariable.transform.position, .35f), "SetNumberValue");
        }

        if (this.overrideOperateNumberValue != null)
        {
            this.onDrawGizmosTriggerEvent.DrawLine(this.overrideOperateNumberValue.transform.position, this.transform.position, this.overrideOperateNumberValue.color);
            base.DrawName(Vector3.Lerp(this.transform.position, this.overrideOperateNumberValue.transform.position, .35f), "SetOperateNumberValue");
        }

        base.OnDrawGizmos();
    }
}
