using UnityEngine;

public class _556mmBullet : Bullet
{
    public override float hpDamage { get ; set ; }
    public override float impactDamage { get ; set ; }
    public override float recoilKickBack { get; set; }
    public override BulletType myType { get; set; }
    public _556mmBullet(Weapon weapon):base(weapon)
    {
        hpDamage = 16.65f;
        impactDamage = 20f;
        recoilKickBack = 180;
        myType = BulletType._556mm;
    }

    //public override void Execute(Vector3 spawnerPosition, Vector3 pointPos,)
    //{
    //    base.Execute(spawnerPosition, pointPos);
    //}
}
