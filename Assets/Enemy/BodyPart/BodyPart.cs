using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BodyPart : MonoBehaviour
{
    [SerializeField] protected Enemy enemy;
    public abstract void GotHit(float damage);
   
}
