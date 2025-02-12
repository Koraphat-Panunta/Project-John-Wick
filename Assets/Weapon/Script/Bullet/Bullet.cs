using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet:IDamageVisitor
{
    public abstract float hpDamage { get; set; }
    public abstract float impactDamage { get; set; }
    public abstract float recoilKickBack { get; set; }

    protected virtual float bulletHitForce { get; set; }

    protected LayerMask hitLayer;
    protected const float MAX_DISTANCE = 1000;
    public abstract BulletType myType { get; set; } 
    protected Weapon weapon { get; set; }

    public Bullet(Weapon weapon)
    {
        bulletHitForce = 40;
        this.weapon = weapon;
    }
    public virtual Vector3 Shoot(Vector3 spawnerPosition,Vector3 pointPos)
    {
        int DefaultMask = LayerMask.GetMask("Default");
        int BodyPartMask = LayerMask.GetMask("Enemy");
        int PlayerHitMask = LayerMask.GetMask("Player");
        hitLayer = DefaultMask + BodyPartMask + PlayerHitMask;
        // Calculate and apply impulse force
        Vector3 force = (pointPos-spawnerPosition).normalized;
        Vector3 rayDir = (pointPos - spawnerPosition).normalized;
        Ray ray = new Ray(spawnerPosition,rayDir);
        if (Physics.Raycast(ray,out RaycastHit hit,MAX_DISTANCE,hitLayer))
        {
            HitExecute(hit,rayDir);
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
