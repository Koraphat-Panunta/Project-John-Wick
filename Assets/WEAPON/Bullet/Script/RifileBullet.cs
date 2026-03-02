using UnityEngine;

public class RifileBullet : Bullet
{

    public override BulletType myType { get;protected set; }
    public override float _hPDamage { get => this.weapon.weaponStatsScriptableObject._hpDamage; set { } }
    public override float _postureDamageVisitor { get => this.weapon.weaponStatsScriptableObject._postureDamage; set { } }
    public override float _pureDestructionDamage { get => this.weapon.weaponStatsScriptableObject._destructionDamage; set { } }
    public override float maxPenetrateRate => 1f;

    public RifileBullet(Weapon weapon):base(weapon)
    {
        myType = BulletType.rifleAmmo;
    }

    //public override void Execute(Vector3 spawnerPosition, Vector3 pointPos,)
    //{
    //    base.Execute(spawnerPosition, pointPos);
    //}
}
