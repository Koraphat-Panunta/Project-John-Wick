using UnityEngine;

public class _556mmBullet : Bullet
{
    public override float _hpDamage { get ; set ; }
    public override float _postureDamage { get ; set ; }
    public override float recoilKickBack { get; set; }
    public override BulletType myType { get; set; }
    public override float _destructionDamage { get; set; }

    public _556mmBullet(Weapon weapon):base(weapon)
    {
        _hpDamage = 16.65f;
        _postureDamage = 20f;
        _destructionDamage = 17;
        recoilKickBack = 180;
        myType = BulletType._556mm;
    }

    //public override void Execute(Vector3 spawnerPosition, Vector3 pointPos,)
    //{
    //    base.Execute(spawnerPosition, pointPos);
    //}
}
