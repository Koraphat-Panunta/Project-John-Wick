using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BodyPart : MonoBehaviour,IBulletDamageAble,IGunFuDamagedAble
{
    [SerializeField] public Enemy enemy;
    public abstract float hpReciverRate { get; set; }
    public abstract float postureReciverRate { get; set; }
    public bool _triggerHitedGunFu { get; set; }
    public Transform _gunFuHitedAble { get; set; }
    public Vector3 attackedPos {get;set; }

    public abstract void TakeDamage(IDamageVisitor damageVisitor);
    public abstract void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart);

    public void TakeGunFuAttacked(GunFuHitNodeLeaf gunFu_NodeLeaf, Vector3 attackerPos)
    {
        enemy.TakeGunFuAttacked(gunFu_NodeLeaf, attackerPos);
    }

    protected void HitsensingTarget(Vector3 hitPart)
    {
        if (enemy.fieldOfView.FindSingleObjectInView(enemy.targetMask, (new Vector3(hitPart.x,0,hitPart.z) - enemy.transform.position).normalized, 120, out GameObject targetObj))
        {
            enemy.targetKnewPos = targetObj.transform.position;
        }
    }
   
}
