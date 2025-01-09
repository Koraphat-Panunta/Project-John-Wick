using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBodyPart : BodyPart
{
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        Bullet bulletObj = damageVisitor as Bullet;   
        float damage = bulletObj.hpDamage;

        //_enemy.enemyStateManager.ChangeState(_enemy.enemyStateManager._painState, new BodyHitNormalReaction(_enemy));
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GetShoot_Head);
        enemy.TakeDamage(damage * 6);
    }
}
