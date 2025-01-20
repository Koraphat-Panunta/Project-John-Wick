using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BodyPart : MonoBehaviour,IBulletDamageAble,IGunFuDamagedAble
{
    [SerializeField] public Enemy enemy;
    public abstract float hpReciverRate { get; set; }
    public abstract float postureReciverRate { get; set; }
    public bool _triggerHitedGunFu { get; set; }
    public Transform _gunFuHitedAble { get => enemy._gunFuHitedAble; set { } }
    public Vector3 attackedPos {get;set; }

    public abstract void TakeDamage(IDamageVisitor damageVisitor);
    public abstract void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart);

    public void TakeGunFuAttacked(GunFuHitNodeLeaf gunFu_NodeLeaf, IGunFuAble attackerPos)
    {
        enemy.TakeGunFuAttacked(gunFu_NodeLeaf, attackerPos);
    }

    public void TakeGunFuAttacked(GunFu_Interaction_NodeLeaf gunFu_Interaction_NodeLeaf, IGunFuAble gunFuAble)
    {
        enemy.TakeGunFuAttacked(gunFu_Interaction_NodeLeaf, gunFuAble);
    }

    protected void HitsensingTarget(Vector3 hitPart)
    {
        if (enemy.fieldOfView.FindSingleObjectInView(enemy.targetMask, (new Vector3(hitPart.x,0,hitPart.z) - enemy.transform.position).normalized, 120, out GameObject targetObj))
        {
            enemy.targetKnewPos = targetObj.transform.position;
        }
    }
   
}
