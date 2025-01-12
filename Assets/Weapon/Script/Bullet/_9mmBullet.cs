using UnityEngine;

public class _9mmBullet : Bullet
{
    public override float hpDamage { get; set ; }
    public override float impactDamage { get ; set ; }
    public override BulletType myType { get; set; }
    public override float recoilKickBack { get; set; }
    public _9mmBullet()
    {
        hpDamage = 40;
        impactDamage = 30;
        myType = BulletType._9mm;
        recoilKickBack = 30;
    }
    //public override void ShootDirection(Vector3 spawnerPosition, Vector3 pointPos)
    //{
    //    base.ShootDirection(spawnerPosition, pointPos);
    //}
    protected override void HitExecute(RaycastHit hit)
    {
        base.HitExecute(hit);
    }
}
