using UnityEngine;

public class FloatVirtualVariable : NumberVirtualVariable
{
    [SerializeField] protected float value;
    public override float _value => this.value;
}
