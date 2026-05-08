using UnityEngine;

public class HandgunBullet : Bullet
{

    public override BulletType myType { get; protected set; }
    public override float _hPDamage { get => this.weapon.weaponStatsScriptableObject._hpDamage; set { } }
    public override float _postureDamageVisitor { get => this.weapon.weaponStatsScriptableObject._postureDamage; set { } }
    public override float _pureDestructionDamage { get => this.weapon.weaponStatsScriptableObject._destructionDamage; set { } }

    public HandgunBullet(RangeWeapon weapon):base(weapon)
    {
        myType = BulletType.handgunAmmo;
    }
    
   
}
