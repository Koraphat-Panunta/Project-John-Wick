using UnityEngine;

public class _9mmBullet : Bullet
{
    public override float hpDamage { get; set ; }
    public override float impactDamage { get ; set ; }
    public override BulletType myType { get; set; }
    public override float recoilKickBack { get; set; }
    public _9mmBullet(Weapon weapon):base(weapon)
    {
        hpDamage = 12.5f;
        impactDamage = 25f;
        myType = BulletType._9mm;
        recoilKickBack = 140;
    }
    //public override void Execute(Vector3 spawnerPosition, Vector3 pointPos)
    //{
    //    base.Execute(spawnerPosition, pointPos);
    //}
   
}
