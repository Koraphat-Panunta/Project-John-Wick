using UnityEngine;

public class ArmRightBodyPart : BodyPart
{

    protected override void Start()
    {
       
        base.Start();
    }
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        enemy._painPart = IPainStateAble.PainPart.ArmRight;
        base.TakeDamage(damageVisitor);
    }

    public override void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
    {
        this.TakeDamage(damageVisitor);
        base.TakeDamage(damageVisitor, hitPart, hitDir, hitforce);
    }
}
