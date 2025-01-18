using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BodyPart : MonoBehaviour,IBulletDamageAble
{
    [SerializeField] public Enemy enemy;
    public abstract float hpReciverRate { get; set; }
    public abstract float postureReciverRate { get; set; }
    public abstract void TakeDamage(IDamageVisitor damageVisitor);
    public abstract void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart);
    protected void HitsensingTarget(Vector3 hitPart)
    {
        if (enemy.fieldOfView.FindSingleObjectInView(enemy.targetMask, (new Vector3(hitPart.x,0,hitPart.z) - enemy.transform.position).normalized, 120, out GameObject targetObj))
        {
            enemy.targetKnewPos = targetObj.transform.position;
        }
    }
   
}
