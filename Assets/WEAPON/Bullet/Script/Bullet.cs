using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class Bullet:IDamageVisitor,INoiseMakingAble
{
    public abstract float _hpDamage { get; set; }
    public abstract float _postureDamage { get; set; }
    public abstract float _destructionDamage { get; set; }
    public abstract float recoilKickBack { get; set; }

    protected virtual float bulletHitForce { get; set; }

    protected LayerMask hitLayer;
    protected const float MAX_DISTANCE = 1000;
    public abstract BulletType myType { get; set; } 
    public Weapon weapon { get; protected set; }
    public Vector3 position { get => weapon.bulletSpawnerPos.position; set { } }
    public NoiseMakingBehavior noiseMakingBehavior { get ; set ; }
    public Action<Collider,Vector3,Vector3> bulletHitNotify;

    public Bullet(Weapon weapon)
    {
        bulletHitForce = 40;
        this.weapon = weapon;
        noiseMakingBehavior = new NoiseMakingBehavior(this);

    }
    private readonly float maxAngle = 45f;
    public virtual Vector3 Shoot(BulletSpawner bulletSpawner, Vector3 pointPos)
    {
        // Normalize input
        Vector3 dirToPoint = (pointPos - bulletSpawner.transform.position).normalized;

        // Basis: forward, right, up
        Vector3 fwd = bulletSpawner.transform.forward.normalized;
        Vector3 right = Vector3.Cross(Vector3.up, fwd).normalized;
        Vector3 up = Vector3.Cross(fwd, right).normalized;

        // Project onto local basis (dot products give angles)
        float horizontalAngle = Mathf.Atan2(Vector3.Dot(dirToPoint, right), Vector3.Dot(dirToPoint, fwd)) * Mathf.Rad2Deg;
        float verticalAngle = (Mathf.Atan2(Vector3.Dot(dirToPoint, up), Vector3.Dot(dirToPoint, fwd)) * Mathf.Rad2Deg) * -1;


        // Clamp angles
        horizontalAngle = Mathf.Clamp(horizontalAngle, - this.maxAngle, this.maxAngle);
        verticalAngle = Mathf.Clamp(verticalAngle, -this.maxAngle, this.maxAngle);

        // Rebuild direction from clamped angles
        Quaternion rot = Quaternion.AngleAxis(horizontalAngle, Vector3.up) *
                         Quaternion.AngleAxis(verticalAngle, right);
        Vector3 clampedDir = (rot * fwd).normalized;

        int DefaultMask = LayerMask.GetMask("Default");
        int BodyPartMask = LayerMask.GetMask("Enemy");
        int PlayerHitMask = LayerMask.GetMask("Player");
        int GroundHitMask = LayerMask.GetMask("Ground");
        hitLayer = DefaultMask + BodyPartMask + PlayerHitMask+ GroundHitMask;

        noiseMakingBehavior.VisitAllHeardingAbleInRaduis(19,BodyPartMask);

        // Calculate and apply impulse force
        Vector3 force = clampedDir;
        Vector3 rayDir = clampedDir;
        Ray ray = new Ray(bulletSpawner.transform.position,rayDir);
        if (Physics.SphereCast(ray,0.015f,out RaycastHit hit,MAX_DISTANCE,hitLayer, QueryTriggerInteraction.Ignore))
        {
            if (bulletHitNotify != null)
                bulletHitNotify.Invoke(hit.collider,hit.point,rayDir);
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
            weapon.userWeapon._weaponAfterAction.SendFeedBackWeaponAfterAction
                <IBulletDamageAble>(WeaponAfterAction.WeaponAfterActionSending.HitConfirm,damageAble);
        }
        

    }
   
}
