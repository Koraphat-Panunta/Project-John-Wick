using UnityEngine;

public class EnemyBodyBulletDamageAbleBehavior : IBulletDamageAble
{
    private BodyPart enemyBody;
    public EnemyBodyBulletDamageAbleBehavior(BodyPart enemyBody)
    {
        this.enemyBody = enemyBody;
    }

    public float penatrateResistance { get => enemyBody.penatrateResistance; }

    public virtual void TakeDamageBullet(IDamageVisitor damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce)
    {
        enemyBody.enemy.forceSave = hitDir * hitforce*0.03f;
        enemyBody.StackingForce(hitDir*hitforce*1.5f,hitPos);

        enemyBody.enemy.NotifyObserver<BulletHitDetail>(enemyBody.enemy,new BulletHitDetail { hitPos = hitPos, hitDir = hitDir, hitforce = hitforce });
    }

    public void TakeDamage(IDamageVisitor damageVisitor)
    {
       
    }

    public struct BulletHitDetail
    {
        public Vector3 hitPos;
        public Vector3 hitDir;
        public float hitforce;
    }
}
