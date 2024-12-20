using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBodyPart : BodyPart
{
    public override void GotHit(float damage)
    {
        enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._painState, new BodyHitNormalReaction(enemy));
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GetShoot_Head);
        enemy.TakeDamage(damage*6);

    }
}
