using UnityEngine;
using static SubjectPlayer;

public class PlayerBulletDamageAbleBehavior : IBulletDamageAble
{
    protected IBulletDamageAble bulletDamageAble;
    protected Player player;

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
    }
    public virtual void TakeDamage(IDamageVisitor damageVisitor)
    {

        Bullet bulletObj = damageVisitor as Bullet;
        float damage = bulletObj.GetHpDamage;

        player.SetHP(player.GetHP() - damage * 1f);
        player.NotifyObserver(this.player, NotifyEvent.GetDamaged);
    }
    public virtual void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce)
    {
        damageDetail.damageVisitor = damageVisitor;
        damageDetail.hitPos = hitPos;
        damageDetail.hitDir = hitDir;
        damageDetail.hitforce = hitforce;
        player.NotifyObserver(this.player, NotifyEvent.GetShoot);

        player.TakeDamage(damageVisitor);
    }

}
