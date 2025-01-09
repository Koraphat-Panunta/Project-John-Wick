using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBodyPart : BodyPart
{
   
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        Bullet bulletObj = damageVisitor as Bullet;
        float damage = bulletObj.hpDamage;

        if (enemy.pressure > 0)
        {
            enemy.pressure -= damage * Random.Range(4, 6);
            enemy.enemyMiniFlinch.TriggerFlich();
            enemy.TakeDamage(damage * 0.4f);
        }
        else
        {
            //_enemy.enemyStateManager.ChangeState(_enemy.enemyStateManager._painState, new BodyHitNormalReaction(_enemy));
            enemy.TakeDamage(damage);
        }
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GetShoot_Chest);
    }
}
