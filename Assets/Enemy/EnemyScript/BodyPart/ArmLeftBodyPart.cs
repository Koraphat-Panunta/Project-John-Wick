using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmLeftBodyPart : BodyPart
{
    public override float hpReciverMultiplyRate { get; set; }
    public override float postureReciverRate { get; set; }
    protected override void Start()
    {
        hpReciverMultiplyRate = 0.25f;
        postureReciverRate = 0.9f;
        base.Start();
    }

    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        Bullet bulletObj = damageVisitor as Bullet;

        float damage = bulletObj.hpDamage * hpReciverMultiplyRate;
        float pressureDamage = bulletObj.impactDamage * postureReciverRate;

        enemy._isPainTrigger = true;
        enemy._painPart = IPainStateAble.PainPart.ArmLeft;

        if(enemy._posture > 0)
            enemy._posture -= pressureDamage ;

        enemy.TakeDamage(damage);

        //enemyBody.NotifyObserver(enemyBody, SubjectEnemy.EnemyEvent.GetShoot_Arm);
    }

    public override void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
    {

        TakeDamage(damageVisitor);

        base.TakeDamage(damageVisitor, hitPart, hitDir, hitforce);
    }
}
