using UnityEngine;
using static EnemyBodyBulletDamageAbleBehavior;

public class ChestBodyPart : BodyPart,IGotGunFuAttackedAble
{
    
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        base.TakeDamage(damageVisitor);
    }
    public void TakeGunFuAttacked(IGunFuNode gunFu_NodeLeaf, IGunFuAble attackerPos)
    {
        enemy.TakeGunFuAttacked(gunFu_NodeLeaf, attackerPos);
        if(gunFu_NodeLeaf is GunFuHitNodeLeaf gunFuHitNodeLeaf)
        {
            CharacterHitedEventDetail characterHitedEventDetail = new CharacterHitedEventDetail
            {
                hitDir = gunFuHitNodeLeaf.hitDir,
                hitedPart = this,
                hitforce = gunFuHitNodeLeaf.gunFuHitScriptableObject.hitPushForce[gunFuHitNodeLeaf.hitCount],
                hitPos = this.transform.position
            };

            this.enemy.NotifyObserver<CharacterHitedEventDetail>(this.enemy, characterHitedEventDetail);
           
        }
       
    }

    public override void TakeDamageBullet(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
    {

        this.TakeDamage(damageVisitor);
        base.TakeDamageBullet(damageVisitor, hitPart, hitDir, hitforce);
    }
   
    public override void OnNotify<T>(Enemy enemy, T node)
    {
        base.OnNotify(enemy, node);
    }
}
