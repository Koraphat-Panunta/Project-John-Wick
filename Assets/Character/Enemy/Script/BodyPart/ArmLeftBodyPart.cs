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
        enemy._painPart = IPainStateAble.PainPart.ArmLeft;
        base.TakeDamage(damageVisitor);
    }

    public override void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
    {
        this.TakeDamage(damageVisitor);
        base.TakeDamage(damageVisitor, hitPart, hitDir, hitforce);
    }
}
