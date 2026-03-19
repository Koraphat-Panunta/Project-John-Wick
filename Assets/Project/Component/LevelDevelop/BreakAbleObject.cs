using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BreakAbleObject : MonoBehaviour, IDamageAble
{
    [SerializeField] protected float hp;

    public abstract void TakeDamage(IDamageVisitor damageVisitor);
}

