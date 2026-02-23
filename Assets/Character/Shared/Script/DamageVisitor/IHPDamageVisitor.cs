using UnityEngine;

public interface IHPDamageVisitor : IDamageVisitor
{
    public float _hPDamage { get; }
}
