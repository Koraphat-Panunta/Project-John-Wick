using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmBodyPart : BodyPart
{
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        Bullet bulletObj = damageVisitor as Bullet;
        float damage = bulletObj.hpDamage;
        if (enemy.pressure > 0)
        {
            enemy.pressure -= damage * Random.Range(3, 4);
            enemy.enemyMiniFlinch.TriggerFlich();
            enemy.TakeDamage(damage * 0.2f);
        }
        else
        {
            enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._painState, new BodyHitNormalReaction(enemy));
            enemy.TakeDamage(damage * 0.7f);
        }

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GetShoot_Arm);
    }
}
