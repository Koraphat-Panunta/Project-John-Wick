using UnityEngine;

public class LegRightBodyPart : BodyPart
{
    
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        base.TakeDamage(damageVisitor);
    }
    public override void TakeDamageBullet(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
    {

        TakeDamage(damageVisitor);

        base.TakeDamageBullet(damageVisitor, hitPart, hitDir, hitforce);
    }
}
