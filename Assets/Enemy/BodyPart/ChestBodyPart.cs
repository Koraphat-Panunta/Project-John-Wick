using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBodyPart : BodyPart
{
    public override void GotHit(float damage)
    {
        if (enemy.pressure > 0)
        {
            enemy.pressure -= damage*Random.Range(4,6);
            enemy.enemyMiniFlinch.TriggerFlich();
            enemy.TakeDamage(damage*0.4f);
        }
        else
        {
            enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._painState, new BodyHitNormalReaction(enemy));
            enemy.TakeDamage(damage);
        }
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GetShoot_Chest);
    }
}
