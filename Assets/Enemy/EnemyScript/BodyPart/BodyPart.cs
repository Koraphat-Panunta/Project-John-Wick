using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BodyPart : MonoBehaviour,IDamageAble
{
    [SerializeField] public Enemy enemy;
    public abstract float hpReciverRate { get; set; }
    public abstract float postureReciverRate { get; set; }
    public abstract void TakeDamage(IDamageVisitor damageVisitor);
    public abstract void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart);
   
}
