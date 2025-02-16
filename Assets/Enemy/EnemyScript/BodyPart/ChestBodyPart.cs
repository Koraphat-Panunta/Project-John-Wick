using UnityEngine;

public class ChestBodyPart : BodyPart
{
    public override float hpReciverRate { get; set; }
    public override float postureReciverRate { get; set; }

    protected override void Start()
    {
        hpReciverRate = 1.0f;
        postureReciverRate = 1.0f;
        base.Start();
    }
    
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        Bullet bulletObj = damageVisitor as Bullet;

        float damage = bulletObj.hpDamage * hpReciverRate;
        float pressureDamage = bulletObj.impactDamage * postureReciverRate;

        enemy._isPainTrigger = true;
        enemy._painPart = IPainStateAble.PainPart.BodyFornt;

        if (enemy._posture > 0)
            enemy._posture -= pressureDamage;

        enemy.TakeDamage(damage);
    }

    public override void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
    {
        HitsensingTarget(hitPart);


        Bullet bulletObj = damageVisitor as Bullet;

        float damage = bulletObj.hpDamage * hpReciverRate;
        float pressureDamage = bulletObj.impactDamage * postureReciverRate;

        float dot = Vector3.Dot(enemy.transform.forward,
            new Vector3(hitPart.x - enemy.transform.position.x, 0, hitPart.z - enemy.transform.position.z).normalized);

        enemy._isPainTrigger = true;

        if (enemy._posture > 0)
            enemy._posture -= pressureDamage;

        if (dot>=0)
            enemy._painPart = IPainStateAble.PainPart.BodyFornt;
        else
            enemy._painPart = IPainStateAble.PainPart.BodyBack;

        enemy.TakeDamage(damage);

        base.TakeDamage(damageVisitor, hitPart, hitDir, hitforce);
    }
}
