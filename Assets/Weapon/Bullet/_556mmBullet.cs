using UnityEngine;

public class _556mmBullet : Bullet
{
    public override float hpDamage { get ; set ; }
    public override float impactDamage { get ; set ; }
    public override float recoilKickBack { get; set; }
    public override BulletType myType { get; set; }
    public _556mmBullet()
    {
        hpDamage = 36;
        impactDamage = 40;
        recoilKickBack = 60;
        myType = BulletType._556mm;
    }

    public override void ShootDirection(Vector3 spawnerPosition, Vector3 pointPos)
    {
        base.ShootDirection(spawnerPosition, pointPos);
    }

    protected override void HitExecute(RaycastHit hit)
    {
        base.HitExecute(hit);
    }
}
