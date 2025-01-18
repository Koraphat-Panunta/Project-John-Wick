using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmLeftBodyPart : BodyPart
{
    public override float hpReciverRate { get; set; }
    public override float postureReciverRate { get; set; }
    private void Start()
    {
        hpReciverRate = 0.25f;
        postureReciverRate = 0.9f;
    }

    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        Bullet bulletObj = damageVisitor as Bullet;

        float damage = bulletObj.hpDamage * hpReciverRate;
        float pressureDamage = bulletObj.impactDamage * postureReciverRate;

        enemy._isPainTrigger = true;
        enemy._painPart = IPainState.PainPart.ArmLeft;

        if(enemy.posture > 0)
            enemy.posture -= pressureDamage ;

        enemy.TakeDamage(damage);

        //enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GetShoot_Arm);
    }

    public override void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart)
    {
        HitsensingTarget(hitPart);
            TakeDamage(damageVisitor);
    }
}
