using UnityEngine;

public class Glass : 
    BreakAbleObject
    ,IBulletDamageAble
{
    public float penatrateResistance => 0;

    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        
    }

    public void TakeDamageBullet(IDamageVisitor damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce)
    {
        
    }
}
