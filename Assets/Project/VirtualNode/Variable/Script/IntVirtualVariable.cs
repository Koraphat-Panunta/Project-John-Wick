using UnityEngine;

public class IntVirtualVariable : NumberVirtualVariable
{
    [SerializeField] protected int value;

    public override float _value { get => this.value; set => this.value = (int)value; }
}
