using UnityEngine;

public class _556mmBullet : Bullet
{

    public override BulletType myType { get; set; }
    public override float _pureHpDamage { get; set; }
    public override float _purePostureDamage { get; set; }
    public override float _pureDestructionDamage { get; set; }
    public override float maxPenetrateRate => 1f;

    public _556mmBullet(Weapon weapon):base(weapon)
    {
        _pureHpDamage = 18f;
        _purePostureDamage = 20f;
        _pureDestructionDamage = 17;
        myType = BulletType._556mm;
    }

    //public override void Execute(Vector3 spawnerPosition, Vector3 pointPos,)
    //{
    //    base.Execute(spawnerPosition, pointPos);
    //}
}
