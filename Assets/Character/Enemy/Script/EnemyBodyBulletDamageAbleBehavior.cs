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

        enemyBody.enemy.NotifyObserver<CharacterHitedEventDetail>(
            this.enemyBody.enemy
            ,new CharacterHitedEventDetail 
            { 
                hitedPart = enemyBody
                , hitPos = hitPos
                , hitDir = hitDir
                , hitforce = hitforce 
            }
            );
    }

    public void TakeDamage(IDamageVisitor damageVisitor)
    {
       
    }

    public struct CharacterHitedEventDetail
    {
        public BodyPart hitedPart;
        public Vector3 hitPos;
        public Vector3 hitDir;
        public float hitforce;
    }
}
