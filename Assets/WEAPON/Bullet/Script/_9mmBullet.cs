using UnityEngine;

public class _9mmBullet : Bullet
{
    public override float _hpDamage { get; set ; }
    public override float _postureDamage { get ; set ; }
    public override BulletType myType { get; set; }
    public override float recoilKickBack { get; set; }
    public override float _destructionDamage { get; set; }

    public _9mmBullet(Weapon weapon):base(weapon)
    {
        _hpDamage = 17f;
        _postureDamage = 18.65f;
        _destructionDamage = 15;
        myType = BulletType._9mm;
        recoilKickBack = 140;
    }
    //public override void Execute(Vector3 spawnerPosition, Vector3 pointPos)
    //{
    //    base.Execute(spawnerPosition, pointPos);
    //}
   
}
