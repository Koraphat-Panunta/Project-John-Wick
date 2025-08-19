using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegLeftBodyPart : BodyPart
{


    protected override void Start()
    {
        base.Start();
    }
    
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        enemy._painPart = IPainStateAble.PainPart.LegLeft;
        base.TakeDamage(damageVisitor);
    }
    public override void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
    {

        TakeDamage(damageVisitor);

        base.TakeDamage(damageVisitor, hitPart, hitDir, hitforce);
    }
}
