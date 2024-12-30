using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBodyPart : BodyPart
{
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        Bullet bulletObj = damageVisitor as Bullet;   
        float damage = bulletObj.hpDamage;

        enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._painState, new BodyHitNormalReaction(enemy));
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GetShoot_Head);
        enemy.TakeDamage(damage * 6);
    }
}
