using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegBodyPart : BodyPart
{
   
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        Bullet bulletObj = damageVisitor as Bullet;
        float damage = bulletObj.hpDamage;

        if (enemy.pressure > 0)
        {
            enemy.pressure -= damage * Random.Range(1, 2);
        }
        //_enemy.enemyStateManager.ChangeState(_enemy.enemyStateManager._painState, new LegHitNormalReaction(_enemy));
        enemy.TakeDamage(damage * 0.5f);
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GetShoot_Leg);
    }
}
