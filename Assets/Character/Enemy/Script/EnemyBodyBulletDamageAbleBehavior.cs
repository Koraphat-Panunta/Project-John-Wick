using UnityEngine;

public static class EnemyBodyBulletDamageAbleBehavior 
{
    public static void TakeDamageBullet(Bullet damageVisitor,BodyPart enemyBody, Vector3 hitPos, Vector3 hitDir, float hitforce)
    {
        enemyBody.enemy.forceSave = hitDir * hitforce*0.03f;
        enemyBody.StackingForce(hitDir*hitforce*1.5f,hitPos);

        enemyBody.enemy.NotifyObserver<CharacterHitedEventDetail>(
            enemyBody.enemy
            ,new CharacterHitedEventDetail 
            { 
                hitedPart = enemyBody
                , hitPos = hitPos
                , hitDir = hitDir
                , hitforce = hitforce 
            }
            );
    }

    public struct CharacterHitedEventDetail
    {
        public BodyPart hitedPart;
        public Vector3 hitPos;
        public Vector3 hitDir;
        public float hitforce;
    }
}
