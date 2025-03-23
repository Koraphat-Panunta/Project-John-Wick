using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet:IDamageVisitor,INoiseMakingAble
{
    public abstract float hpDamage { get; set; }
    public abstract float impactDamage { get; set; }
    public abstract float recoilKickBack { get; set; }

    protected virtual float bulletHitForce { get; set; }

    protected LayerMask hitLayer;
    protected const float MAX_DISTANCE = 1000;
    public abstract BulletType myType { get; set; } 
    public Weapon weapon { get; protected set; }
    public Vector3 position { get => weapon.bulletSpawnerPos.position; set { } }
    public NoiseMakingBehavior noiseMakingBehavior { get ; set ; }

    public Bullet(Weapon weapon)
    {
        bulletHitForce = 60;
        this.weapon = weapon;
        noiseMakingBehavior = new NoiseMakingBehavior(this);
    }
    public virtual Vector3 Shoot(Vector3 spawnerPosition,Vector3 pointPos)
    {

        int DefaultMask = LayerMask.GetMask("Default");
        int BodyPartMask = LayerMask.GetMask("Enemy");
        int PlayerHitMask = LayerMask.GetMask("Player");
        int GroundHitMask = LayerMask.GetMask("Ground");
        hitLayer = DefaultMask + BodyPartMask + PlayerHitMask+ GroundHitMask;

        noiseMakingBehavior.VisitAllHeardingAbleInRaduis(19,BodyPartMask);

        // Calculate and apply impulse force
        Vector3 force = (pointPos-spawnerPosition).normalized;
        Vector3 rayDir = (pointPos - spawnerPosition).normalized;
        Ray ray = new Ray(spawnerPosition,rayDir);
        if (Physics.SphereCast(ray,0.015f,out RaycastHit hit,MAX_DISTANCE,hitLayer))
        {
            HitExecute(hit,rayDir);
        }
        else
        {
            return ray.GetPoint(MAX_DISTANCE);
        }
        return hit.point;
    }
    protected virtual void HitExecute(RaycastHit hit,Vector3 dir)
    {
        Collider collider = hit.collider;
        if(collider.TryGetComponent<IBulletDamageAble>(out IBulletDamageAble damageAble))
        {
            damageAble.TakeDamage(this,hit.point,dir,bulletHitForce);
            weapon.userWeapon.weaponAfterAction.HitDamageAble(damageAble);
        }
        

    }
   
}
