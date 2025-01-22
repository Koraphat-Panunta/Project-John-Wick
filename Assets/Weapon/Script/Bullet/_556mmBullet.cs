using UnityEngine;

public class _556mmBullet : Bullet
{
    public override float hpDamage { get ; set ; }
    public override float impactDamage { get ; set ; }
    public override float recoilKickBack { get; set; }
    public override BulletType myType { get; set; }
    public _556mmBullet(Weapon weapon):base(weapon)
    {
        hpDamage = 14;
        impactDamage = 25;
        recoilKickBack = 60;
        myType = BulletType._556mm;
    }

    //public override void ShootDirection(Vector3 spawnerPosition, Vector3 pointPos,)
    //{
    //    base.ShootDirection(spawnerPosition, pointPos);
    //}
}
