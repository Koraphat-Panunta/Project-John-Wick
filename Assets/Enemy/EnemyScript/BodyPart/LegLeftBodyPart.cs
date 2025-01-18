using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegLeftBodyPart : BodyPart
{
    public override float hpReciverRate { get; set; }
    public override float postureReciverRate { get; set; }

    private void Start()
    {
        hpReciverRate = 0.5f;
        postureReciverRate = 2f;
    }
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        Bullet bulletObj = damageVisitor as Bullet;

        float damage = bulletObj.hpDamage * hpReciverRate;
        float pressureDamage = bulletObj.impactDamage * postureReciverRate;

        enemy._isPainTrigger = true;
        enemy._painPart = IPainState.PainPart.LegLeft;

        if (enemy.posture > 0)
            enemy.posture -= pressureDamage;

        enemy.TakeDamage(damage);
    }
    public override void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart)
    {
        HitsensingTarget(hitPart);

        TakeDamage(damageVisitor);
    }
}
