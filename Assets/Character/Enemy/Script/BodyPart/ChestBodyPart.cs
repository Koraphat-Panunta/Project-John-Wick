using UnityEngine;
using static EnemyBodyBulletDamageAbleBehavior;

public class ChestBodyPart : BodyPart
{
    
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        base.TakeDamage(damageVisitor);
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
