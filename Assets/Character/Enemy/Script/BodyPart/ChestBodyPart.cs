using UnityEngine;

public class ChestBodyPart : BodyPart
{


    protected override void Start()
    {
        base.Start();
    }
    
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
    public override void Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
        base.Notify(enemy, enemyEvent);
    }
    public override void Notify<T>(Enemy enemy, T node)
    {
        if (node is EnemyStateLeafNode enemyStateLeafNode)
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
