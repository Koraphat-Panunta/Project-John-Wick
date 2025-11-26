using UnityEngine;
using static SubjectPlayer;

public class PlayerBulletDamageAbleBehavior : IBulletDamageAble,IObserverPlayer
{
    protected IBulletDamageAble bulletDamageAble;
    protected Player player;
    protected float ignoreBulletChance;

    public struct BulletDamageDetail
    {
        public IDamageVisitor damageVisitor;
        public Vector3 hitPos;
        public Vector3 hitDir;
        public float hitforce;
    }

    public BulletDamageDetail damageDetail;

    public float penatrateResistance => player.penatrateResistance;

    public PlayerBulletDamageAbleBehavior(Player player)
    {
        this.player = player;
        this.bulletDamageAble = player as IBulletDamageAble;
        damageDetail = new BulletDamageDetail();
        player.AddObserver(this);
    }
    public virtual void TakeDamage(IDamageVisitor damageVisitor)
    {

        Bullet bulletObj = damageVisitor as Bullet;

        if(bulletObj.weapon.userWeapon != null 
            && bulletObj.weapon.userWeapon is Player)
            return;

        float damage = bulletObj.GetHpDamage;

        player.SetHP(player.GetHP() - damage * 1f);
        player.NotifyObserver(this.player, NotifyEvent.GetDamaged);
    }
    public virtual void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce)
    {

        Bullet bulletObj = damageVisitor as Bullet;

        if (bulletObj.weapon.userWeapon != null
            && bulletObj.weapon.userWeapon is Player)
            return;

        if (Random.Range(0f,1f) < ignoreBulletChance)
            return;

        damageDetail.damageVisitor = damageVisitor;
        damageDetail.hitPos = hitPos;
        damageDetail.hitDir = hitDir;
        damageDetail.hitforce = hitforce;
        player.NotifyObserver(this.player, NotifyEvent.GetShoot);

        player.TakeDamage(damageVisitor);
    }

    public void OnNotify<T>(Player player, T node)
    {
        if (node is RestrainGunFuStateNodeLeaf restrictNodeLeaf)
        {
            if (restrictNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Enter)
                ignoreBulletChance = 0.25f;

            if (restrictNodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
                ignoreBulletChance = 0;
        }


        if (node is HumanShield_GunFu_NodeLeaf humanShield_GunFuInteraction_NodeLeaf)
        {
            if(humanShield_GunFuInteraction_NodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Enter)
                ignoreBulletChance = 0.5f;

            if(humanShield_GunFuInteraction_NodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
                ignoreBulletChance = 0;
        }
           


            
    }
}
