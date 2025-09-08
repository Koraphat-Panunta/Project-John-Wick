using UnityEngine;

public class EnemyBodyBulletDamageAbleBehavior : IBulletDamageAble
{
    private BodyPart enemyBody;
    public EnemyBodyBulletDamageAbleBehavior(BodyPart enemyBody)
    {
        this.enemyBody = enemyBody;
    }
    public virtual void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce)
    {
        enemyBody.enemy.forceSave = hitDir * hitforce*0.03f;
        enemyBody.StackingForce(hitDir*hitforce*15f,hitPos);
    }

    public void TakeDamage(IDamageVisitor damageVisitor)
    {
       
    }
}
