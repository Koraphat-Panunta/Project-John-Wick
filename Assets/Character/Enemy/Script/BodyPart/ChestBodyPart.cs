using UnityEngine;

public class ChestBodyPart : BodyPart
{
    public override float hpReciverMultiplyRate { get; set; }
    public override float postureReciverRate { get; set; }

    protected override void Start()
    {
        hpReciverMultiplyRate = 1.0f;
        postureReciverRate = 1.0f;
        base.Start();
    }
    
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        Bullet bulletObj = damageVisitor as Bullet;

        float damage = bulletObj.hpDamage * hpReciverMultiplyRate;
        float pressureDamage = bulletObj.impactDamage * postureReciverRate;

        enemy._isPainTrigger = true;
        enemy._painPart = IPainStateAble.PainPart.BodyFornt;

        if (enemy._posture > 0)
            enemy._posture -= pressureDamage;

        enemy.TakeDamage(damage);
    }

    public override void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
    {
        Bullet bulletObj = damageVisitor as Bullet;

        float damage = bulletObj.hpDamage * hpReciverMultiplyRate;
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
    public override void Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
        base.Notify(enemy, enemyEvent);
    }
    public override void Notify<T>(Enemy enemy, T node)
    {
        if(node is EnemyStateLeafNode enemyStateLeafNode)
            switch (enemyStateLeafNode)
            {
                case HumanThrowFallDown_GotInteract_NodeLeaf humanThrowFallDown_GotInteract:
                    {
                        if(humanThrowFallDown_GotInteract.curState == FallDown_EnemyState_NodeLeaf.FallDownState.RagdollState)
                        isForceSave = true;
                        forceSave = humanThrowFallDown_GotInteract.GetForceThrow();
                        break;
                    }
            }
        base.Notify(enemy, node);
    }
}
