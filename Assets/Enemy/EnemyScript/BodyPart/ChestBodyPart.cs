using UnityEngine;

public class ChestBodyPart : BodyPart
{
    public override float hpReciverRate { get; set; }
    public override float postureReciverRate { get; set; }

    private void Start()
    {
        hpReciverRate = 1.0f;
        postureReciverRate = 1.0f;
    }
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        Bullet bulletObj = damageVisitor as Bullet;

        float damage = bulletObj.hpDamage * hpReciverRate;
        float pressureDamage = bulletObj.impactDamage * postureReciverRate;

        enemy._isPainTrigger = true;
        enemy._painPart = IPainState.PainPart.BodyFornt;

        if (enemy.posture > 0)
            enemy.posture -= pressureDamage;

        enemy.TakeDamage(damage);
    }

    public override void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPoint)
    {
        Bullet bulletObj = damageVisitor as Bullet;

        float damage = bulletObj.hpDamage * hpReciverRate;
        float pressureDamage = bulletObj.impactDamage * postureReciverRate;

        float dot = Vector3.Dot(enemy.transform.forward,
            new Vector3(hitPoint.x - enemy.transform.position.x, 0, hitPoint.z - enemy.transform.position.z).normalized);

        enemy._isPainTrigger = true;

        if (enemy.posture > 0)
            enemy.posture -= pressureDamage;

        if (dot>=0)
            enemy._painPart = IPainState.PainPart.BodyFornt;
        else
            enemy._painPart = IPainState.PainPart.BodyBack;

        enemy.TakeDamage(damage);
    }
}
