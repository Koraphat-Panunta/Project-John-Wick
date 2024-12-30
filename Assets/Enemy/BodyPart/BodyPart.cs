using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BodyPart : MonoBehaviour,IDamageAble
{
    [SerializeField] public Enemy enemy;
    public abstract void TakeDamage(IDamageVisitor damageVisitor);
   
}
