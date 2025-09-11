using UnityEngine;

public class _9mmBullet : Bullet
{

    public override BulletType myType { get; set; }
    public override float recoilKickBack { get; set; }
    public override float _pureHpDamage { get; set; }
    public override float _purePostureDamage { get; set; }
    public override float _pureDestructionDamage { get; set; }

    public _9mmBullet(Weapon weapon):base(weapon)
    {
        _pureHpDamage = 17f;
        _purePostureDamage = 18.65f;
        _pureDestructionDamage = 15;
        myType = BulletType._9mm;
        recoilKickBack = 140;
    }
    //public override void Execute(Vector3 spawnerPosition, Vector3 pointPos)
    //{
    //    base.Execute(spawnerPosition, pointPos);
    //}
   
}
