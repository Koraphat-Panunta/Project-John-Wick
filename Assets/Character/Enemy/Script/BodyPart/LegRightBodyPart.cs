using UnityEngine;

public class LegRightBodyPart : BodyPart
{
    public override float hpReciverMultiplyRate { get; set; }
    public override float postureReciverRate { get; set; }

    protected override void Start()
    {
        base.Start();
        hpReciverMultiplyRate = 0.5f;
        postureReciverRate = 2f;
    }
   
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        
        enemy._painPart = IPainStateAble.PainPart.LegRight;
        base.TakeDamage(damageVisitor);
    }
    public override void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
    {

        TakeDamage(damageVisitor);

        base.TakeDamage(damageVisitor, hitPart, hitDir, hitforce);
    }
}
