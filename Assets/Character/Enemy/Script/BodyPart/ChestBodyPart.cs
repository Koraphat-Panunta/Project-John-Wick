using UnityEngine;

public class ChestBodyPart : BodyPart
{
    
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        base.TakeDamage(damageVisitor);
    }

    public override void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
    {

        float dot = Vector3.Dot(enemy.transform.forward,
            new Vector3(hitPart.x - enemy.transform.position.x, 0, hitPart.z - enemy.transform.position.z).normalized);

        if (dot>=0)
            enemy._painPart = IPainStateAble.PainPart.BodyFornt;
        else
            enemy._painPart = IPainStateAble.PainPart.BodyBack;

        this.TakeDamage(damageVisitor);
        base.TakeDamage(damageVisitor, hitPart, hitDir, hitforce);
    }
   
    public override void Notify<T>(Enemy enemy, T node)
    {
        base.Notify(enemy, node);
    }
}
