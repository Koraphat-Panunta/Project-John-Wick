using UnityEngine;

public class HandgunBullet : Bullet
{

    public override BulletType myType { get; protected set; }
    public override float _pureHpDamage { get; set; }
    public override float _purePostureDamage { get; set; }
    public override float _pureDestructionDamage { get; set; }

    public HandgunBullet(Weapon weapon):base(weapon)
    {
        _pureHpDamage = 17f;
        _purePostureDamage = 18.65f;
        _pureDestructionDamage = 15;
        myType = BulletType.handgunAmmo;
    }
    
   
}
