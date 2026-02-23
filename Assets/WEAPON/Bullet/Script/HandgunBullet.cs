using UnityEngine;

public class HandgunBullet : Bullet
{

    public override BulletType myType { get; protected set; }
    public override float _hPDamage { get; set; }
    public override float _postureDamageVisitor { get; set; }
    public override float _pureDestructionDamage { get; set; }

    public HandgunBullet(Weapon weapon):base(weapon)
    {
        _hPDamage = 17f;
        _postureDamageVisitor = 18.65f;
        _pureDestructionDamage = 15;
        myType = BulletType.handgunAmmo;
    }
    
   
}
