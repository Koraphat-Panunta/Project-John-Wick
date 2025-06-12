using UnityEngine;

public class EnemyBodyPartHumanShieldDamageAbleBehavior : IBulletDamageAble
{
    private HumanShield_GunFuInteraction_NodeLeaf humanShield_GunFuInteraction_NodeLeaf;
    public EnemyBodyPartHumanShieldDamageAbleBehavior(HumanShield_GunFuInteraction_NodeLeaf humanShield_GunFuInteraction_Node)
    {
        this.humanShield_GunFuInteraction_NodeLeaf = humanShield_GunFuInteraction_Node;
    }
    public void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce)
    {
        humanShield_GunFuInteraction_NodeLeaf.HumanShieldedOpponentGotShoot(hitPos);
    }

    public void TakeDamage(IDamageVisitor damageVisitor)
    {
        
    }
}
